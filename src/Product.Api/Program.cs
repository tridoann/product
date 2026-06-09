using Microsoft.EntityFrameworkCore;
using Product.Api.Hubs;
using Product.Application.Extensions;
using Product.Application.Services;
using Product.Infrastructure.Extensions;
using Product.Infrastructure.Services;

// Preserve the timezone-agnostic semantics of the previous SQL Server datetime2 columns.
// This lets both DateTime.UtcNow (Kind=Utc) and unspecified-kind values be written to
// PostgreSQL 'timestamp without time zone' columns without Npgsql kind validation errors.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddMemoryCache();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder
       .AllowAnyMethod()
       .AllowAnyHeader()
       .SetIsOriginAllowed(origin => true)
       .AllowCredentials();
}));


builder.Services.AddInfrastructure(configuration);

// Add services to the container.
builder.Services.AddServices();

builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, SignalRNotificationService<NotificationHub>>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider
    .GetRequiredService<Product.Infrastructure.Database.ProductDbContext>();
// await context.Database.EnsureCreatedAsync();
// Check and apply pending migrations
var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
if (pendingMigrations.Any())
{
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()
    || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat").RequireAuthorization();
app.MapHub<NotificationHub>("/hubs/notifications").RequireAuthorization();

await app.RunAsync();


public partial class Program { }