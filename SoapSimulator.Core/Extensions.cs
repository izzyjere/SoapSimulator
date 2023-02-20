global using System.Collections;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Dynamic;
global using System.Runtime.Serialization;
global using System.ServiceModel;
global using System.Xml;
global using System.Xml.Schema;
global using System.Xml.Serialization;

global using Hangfire;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Primitives;

global using SoapCore;
global using SoapCore.Extensibility;
global using SoapCore.ServiceModel;

global using SoapSimulator.Core.Models;
global using SoapSimulator.Core.Services;

namespace SoapSimulator.Core;
public static class Extensions
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

   public static string GetRequestSample(string name)
    {
        return $"""
                 <?xml version="1.0" encoding="utf-8"?>
                 <soapenv:Envelope     
                 	  xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                       xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
                     <soapenv:Body>
                         <syb:ExecuteAction>           
                             <syb:ActionName>{name}</syb:ActionName>                   
                         </syb:ExecuteAction>
                     </soapenv:Body>
                 </soapenv:Envelope>
                 """;
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
            if (!actions.Any(a => a.Request.XMLFileName == Path.GetFileName(file)) && !actions.Any(a => a.Responses.Any(r=>r.XMLFileName  == Path.GetFileName(file))))
            {
                File.Delete(file);
                Console.WriteLine($"Deleted UnusableXMLFiles file {file}");
            }
        }
    }
}
