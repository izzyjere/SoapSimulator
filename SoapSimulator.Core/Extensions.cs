using System.Xml.Serialization;
using System.Xml;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SoapCore;

using SoapSimulator.Core.Models;
using SoapSimulator.Core.Services;
using Hangfire;

namespace SoapSimulator.Core;
public static class Extension
{
    public static IServiceCollection AddSoapSimulatorCore(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();       
        services.AddSingleton<ILogService, ActionLogService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<IActionService, ActionService>();
        services.AddScoped<ISoapService, SimulatorSoapService>();
        services.AddScoped<IXMLValidator,XMLValidatorService>();
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
        var folderPath = Path.Combine(env.WebRootPath, "xml");
        #pragma warning disable CS0618 // Type or member is obsolete
        RecurringJob.AddOrUpdate(()=>DeleteUnusableXMLFiles(folderPath), Cron.MinuteInterval(1));
        #pragma warning restore CS0618 // Type or member is obsolete
        return app;
    }

    public static Stream ToStream(this string @this, object origin)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(@this);
        writer.Flush();
        stream.Position = 0;        
        return stream;
    }
    public static void DeleteUnusableXMLFiles(string folderPath)
    {
        var context = new DatabaseContext();
        var actions = context.SoapActions.ToList();        
        var files = Directory.GetFiles(folderPath);
        foreach (var file in files.Where(f => f.EndsWith(".xml")))
        {
            if (!actions.Any(a => a.Request.XMLFileName == Path.GetFileName(file)) && !actions.Any(a => a.Response.XMLFileName == Path.GetFileName(file)))
            {
                File.Delete(file);
                Console.WriteLine($"Deleted UnusableXMLFiles file {file}");
            }
        }
    }
}
