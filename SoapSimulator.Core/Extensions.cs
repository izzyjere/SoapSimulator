using Microsoft.AspNetCore.Http;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SoapCore;

using SoapSimulator.Core.Models;
using SoapSimulator.Core.Services;

namespace SoapSimulator.Core;
public static class Extensions
{
    public static IServiceCollection AddSoapSimulatorCore(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite("Data Source = sysConfig.db", o =>
            {
                o.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName);
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<IActionService, ActionService>();
        services.AddScoped<ISoapService, SimulatorSoapService>();
        services.AddSingleton<ILogService, ActionLogService>();
        services.AddSoapExceptionTransformer((ex) => ex.Message);
        //services.AddSoapServiceOperationTuner(new SoapOperationTuner());
        services.AddSoapCore();
        return services;
    }
  
}
