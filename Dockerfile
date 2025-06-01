FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

ARG CONFIGURATION=Release

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./src .
COPY ./Directory.Packages.props .
RUN dotnet restore ./**/Product.Api.csproj

# Install the necessary ICU libraries
RUN apk add --no-cache icu-libs icu-data-full

# Publish the app
RUN dotnet publish ./**/Product.Api.csproj --no-restore -c $CONFIGURATION -o /release

# Run time image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

# Set environment variables for globalization support
ENV ASPNETCORE_URLS=http://+:80
ENV LANG=en_US.UTF-8
ENV LC_ALL=en_US.UTF-8
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Install ICU libs in the runtime image to avoid the missing ICU issue
RUN apk add --no-cache icu-libs

WORKDIR /release
COPY --from=build /release .

# Entry point to run the application
ENTRYPOINT ["dotnet", "Product.Api.dll"]
