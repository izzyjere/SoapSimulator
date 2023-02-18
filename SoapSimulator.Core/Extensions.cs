using System.Xml.Serialization;
using System.Xml;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SoapCore;

using SoapSimulator.Core.Models;
using SoapSimulator.Core.Services;

namespace SoapSimulator.Core;
public static partial class Extensions
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
        services.AddSingleton<ILogService, ActionLogService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<IActionService, ActionService>();
        services.AddScoped<ISoapService, SimulatorSoapService>();        
        services.AddSoapExceptionTransformer((ex) => ex.Message);
        services.AddSoapServiceOperationTuner<SoapOperationTuner>();
        services.AddSoapCore();
        return services;
    }
    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<DatabaseContext>();
        var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
        if (context == null || env == null)
        {
            throw new SystemException("No db context was registered");
        }
        context.Database.Migrate();
        var actions = context.SoapActions.ToList();
        var folderPath = Path.Combine(env.WebRootPath, "xml");
        var files = Directory.GetFiles(folderPath);
        foreach (var file in files.Where(f => f.EndsWith(".xml")))
        {
            if (!actions.Any(a => a.Request.XMLFileName == Path.GetFileName(file)) && !actions.Any(a => a.Response.XMLFileName == Path.GetFileName(file)))
            {
                File.Delete(file);
                Console.WriteLine($"Delete useless file {file}");
            }
        }
        return app;
    }

    public static Stream ToStream(this string @this)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(@this);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

}
