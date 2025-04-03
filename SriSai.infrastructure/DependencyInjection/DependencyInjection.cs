using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using SriSai.Application.interfaces.ApiCalls;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Interfaces.Encryption;
using SriSai.infrastructure.ApiCall;
using SriSai.infrastructure.Persistent.DbContext;
using SriSai.infrastructure.Persistent.Services;
using SriSai.Infrastructure.Persistent.Repository;

namespace SriSai.infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Database Configuration
        services.AddDbContext<SriSaiDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repository Pattern
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DatabaseMigrationService>();

        // Security Services
        services.AddSingleton<IHashPassword, BCryptPasswordHasher>();
        services.AddSingleton<IVerifyPassword, BCryptPasswordVerifier>();

        // HTTP Client Factory
        services.AddHttpClient();

        // API Call Services
        services.AddScoped<IMessageSender, MessageSender>();

        return services;
    }
}