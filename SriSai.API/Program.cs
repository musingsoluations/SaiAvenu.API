using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SriSai.API.DTOs.Users.Validation;
using SriSai.API.OpenApiHelper;
using SriSai.API.Services.Auth;
using SriSai.Application.DependencyInjection;
using SriSai.Domain.DependencyInjection;
using SriSai.infrastructure.DependencyInjection;
using SriSai.infrastructure.Persistent.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Configure configuration sources with proper precedence
builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", true, true);

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>(ServiceLifetime.Transient);

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
            // RoleClaimType = "role",
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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDomain();
builder.Services.AddApplication();
if (connectionString != null) builder.Services.AddInfrastructure(connectionString);

// Register JWT token service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

// Initialize database migrations
using var scope = app.Services.CreateScope();
var migrationService = scope.ServiceProvider.GetRequiredService<DatabaseMigrationService>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();