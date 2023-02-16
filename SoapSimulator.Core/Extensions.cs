using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoapSimulator.Core.Models;

namespace SoapSimulator.Core;
public static class Extensions
{
    public static IServiceCollection AddSoapSimulatorCore(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite("Data Source = sysConfig.db");
        });
        return services;
    }
}
