# Running Docker on macOS without Docker Desktop (Apple Silicon)

This guide documents how we run the Docker CLI on macOS using **Colima** (no Docker
Desktop), and the fixes needed to make the **integration tests** (Testcontainers +
SQL Server) work on Apple Silicon (`arm64`).

> Reference we started from: <https://dev.to/mochafreddo/running-docker-on-macos-without-docker-desktop-64o>

---

## 1. Install the Docker CLI + Colima

```bash
# Docker CLI (client only — talks to a Docker engine, but does not ship one)
brew install docker

# Optional: docker compose plugin
brew install docker-compose

# Colima provides the Linux VM + Docker engine that the CLI connects to
brew install colima
```

Start Colima. On Apple Silicon you **must** enable Rosetta so that `amd64`-only
images (like SQL Server) can run under emulation:

```bash
colima start --vz-rosetta --cpu 4 --memory 6
```

| Flag           | Why |
|----------------|-----|
| `--vz-rosetta` | Enables Rosetta x86_64 emulation. Required to run `amd64`-only images on `arm64` (e.g. `mcr.microsoft.com/mssql/server`). Requires `vmType: vz`, which is the default on Apple Silicon. |
| `--cpu 4`      | SQL Server is CPU-hungry; 2 is too few. |
| `--memory 6`   | SQL Server needs **≥ 2 GB just for itself**. The Colima default of 2 GB is not enough. |

These settings are persisted to `~/.colima/default/colima.yaml`, so they survive
`colima stop` / `colima start` and reboots — you only pass the flags once.

Verify:

```bash
docker context ls       # 'colima' should be the active (*) context
docker info             # Server section should show the Linux engine
colima list             # ARCH aarch64, RUNTIME docker, your CPU/MEMORY
```

---

## 2. Fixes we had to apply

We hit two problems that produced misleading errors. Symptoms looked like
"Docker API can't respond", but Docker was actually fine.

### Fix A — Remove the leftover Docker Desktop credential helper

**Symptom** (image pulls fail):

```
docker: error getting credentials - err: exec: "docker-credential-desktop":
executable file not found in $PATH
```

**Cause:** `~/.docker/config.json` still referenced Docker Desktop's credential
store (`"credsStore": "desktop"`), which no longer exists after uninstalling
Docker Desktop.

**Fix:** edit `~/.docker/config.json` and remove the Desktop creds store. Either
delete the line or set it to the macOS keychain helper:

```jsonc
{
  "auths": {},
  // remove:  "credsStore": "desktop",
  // ok to use instead:
  "credsStore": "osxkeychain",
  "currentContext": "colima"
}
```

Verify:

```bash
docker pull hello-world   # should succeed
```

### Fix B — SQL Server is amd64-only; enable Rosetta

**Symptom** (Testcontainers integration tests):

```
Docker.DotNet.DockerApiException : Docker API responded with status code=Conflict,
response={"message":"container <id> is not running"}
   ... Testcontainers.MsSql.MsSqlBuilder.WaitUntil.UntilAsync(MsSqlContainer container)
```

**Cause:** The test image
`mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04` is **amd64-only**, but the
Colima VM runs `aarch64`. Without Rosetta, the container is *created* but the x86
binary crashes immediately. Testcontainers then runs its readiness `exec` against a
container that has already exited → **"container is not running"**.

**Fix:** start Colima with Rosetta (see [section 1](#1-install-the-docker-cli--colima)).
If your VM was created without it, recreate it:

```bash
colima stop
colima start --vz-rosetta --cpu 4 --memory 6
```

> This recreates / reconfigures the Colima VM. Images and containers inside the VM
> are disposable test artifacts and will be re-pulled automatically — nothing in the
> repo is affected.

Verify the SQL Server image actually stays up under emulation:

```bash
docker run -d --name mssqltest --platform linux/amd64 \
  -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=sqlserverintegrationtest@1234!" \
  -p 11433:1433 mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04

sleep 40
docker ps -a --filter name=mssqltest --format '{{.Names}} -> {{.Status}}'  # Up ...
docker logs mssqltest | tail   # look for "Recovery is complete"
docker rm -f mssqltest         # clean up
```

---

## 3. Run the integration tests

```bash
dotnet test test/Product.IntegrationTests
```

Expected:

```
Passed! - Failed: 0, Passed: 5, Skipped: 0, Total: 5
```

---

## Troubleshooting cheat-sheet

| Symptom | Likely cause | Fix |
|---------|--------------|-----|
| `Cannot connect to the Docker daemon` | Colima not running | `colima start` |
| `docker-credential-desktop ... not found` | Stale Desktop creds store | [Fix A](#fix-a--remove-the-leftover-docker-desktop-credential-helper) |
| Testcontainers: `container ... is not running` | amd64 image on arm64 without Rosetta | [Fix B](#fix-b--sql-server-is-amd64-only-enable-rosetta) |
| SQL Server container exits / OOM | Colima memory too low | `colima stop && colima start --memory 6` |
| Testcontainers can't find the Docker socket | Wrong context | `docker context use colima` |
