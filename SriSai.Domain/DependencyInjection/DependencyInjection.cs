using Microsoft.Extensions.DependencyInjection;
using SriSai.Domain.Interface;
using SriSai.Domain.Imp;

namespace SriSai.Domain.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}