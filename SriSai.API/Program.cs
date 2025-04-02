using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Middleware.APM;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using SriSai.API.DTOs.Users.Validation;
using SriSai.API.OpenApiHelper;
using SriSai.API.Services.Auth;
using SriSai.Application.Configuration;
using SriSai.Application.DependencyInjection;
using SriSai.Domain.DependencyInjection;
using SriSai.infrastructure.DependencyInjection;
using SriSai.infrastructure.Persistent.DbContext;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure configuration sources with proper precedence
builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", true, true)
    .AddEnvironmentVariables();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Configure OpenTelemetry for Logging and Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService(builder.Configuration["MW:ServiceName"] ?? "DefaultService"));

        tracing.AddAspNetCoreInstrumentation(); // Auto-trace ASP.NET Core requests
        tracing.AddHttpClientInstrumentation(); // Trace outgoing HTTP calls

        tracing.AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri(builder.Configuration["MW:TargetURL"] ?? "");
            o.Headers = $"x-api-key={builder.Configuration["MW:ApiKey"]}";
        });
    });

//builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;

    logging.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(builder.Configuration["MW:ServiceName"] ?? "DefaultService"));

    logging.AddOtlpExporter(o =>
    {
        o.Endpoint = new Uri(builder.Configuration["MW:TargetURL"] ?? "");
        o.Headers = $"x-api-key={builder.Configuration["MW:ApiKey"]}";
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>(ServiceLifetime.Transient);

builder.Services.ConfigureMWInstrumentation(builder.Configuration);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"] ?? string.Empty)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2) // Allow 2 minute clock drift
        };
    });

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAuthenticatedUser()
            .RequireRole("Admin"));
});

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

// Configure OpenAPI with JWT support
builder.Services.AddOpenApi(options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

// Add layer configurations
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDomain();
builder.Services.AddApplication();
if (connectionString != null)
{
    builder.Services.AddInfrastructure(connectionString);
}

// Register JWT token service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


// Configure WhatsApp settings using options pattern
builder.Services.Configure<WhatsAppConfiguration>(builder.Configuration.GetSection("WhatsApp"));

// Add health checks
builder.Services.AddHealthChecks()
    // Add SQL Server health check
    .AddSqlServer(
        connectionString ?? string.Empty,
        name: "sql-server-connection",
        tags: new[] { "db", "sql", "sqlserver" });

// Add health check UI
builder.Services.AddHealthChecksUI(setup =>
    {
        setup.SetEvaluationTimeInSeconds(60); // Evaluate every 60 seconds
        setup.MaximumHistoryEntriesPerEndpoint(50); // Keep 50 entries in history
        setup.SetApiMaxActiveRequests(3); // Max concurrent requests

        // Add endpoints to monitor
        setup.AddHealthCheckEndpoint("API Health", "/health");
    })
    .AddInMemoryStorage(); // Store health check results in memory

WebApplication app = builder.Build();
Logger.Init(app.Services.GetRequiredService<ILoggerFactory>());

// Initialize database migrations
using IServiceScope scope = app.Services.CreateScope();
DatabaseMigrationService migrationService = scope.ServiceProvider.GetRequiredService<DatabaseMigrationService>();
await migrationService.StartAsync(CancellationToken.None);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Sai Avenue API";
        options.Layout = ScalarLayout.Modern;
        options.DefaultHttpClient =
            new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.ShowSidebar = true;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoints
app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse, AllowCachingResponses = false
    });
app.UseHealthChecks("/",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse, AllowCachingResponses = false
    });

app.MapControllers();

app.Run();