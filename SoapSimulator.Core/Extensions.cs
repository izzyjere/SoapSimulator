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

using Microsoft.Extensions.Logging;

namespace SoapSimulator.Core;
public static class Extensions
{
    public static IServiceCollection AddSoapSimulatorCore(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(); 
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
        return $"http://hostname/soap?actionName={name}";           
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
