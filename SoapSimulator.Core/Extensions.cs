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

using Hangfire.MemoryStorage;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace SoapSimulator.Core;
public static class Extensions
{
    public static IServiceCollection AddSoapSimulatorCore(this IServiceCollection services)
    {
        services.AddHangfire(configuration => configuration
            .UseSerializerSettings(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            })
            .UseMemoryStorage());
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
    public static IApplicationBuilder UseSoapSimulatorCore(this IApplicationBuilder app)
    {
        app.Use((context, next) =>
        {
            var path = context.Request.Path;

            if (path.HasValue && path.StartsWithSegments("/soap"))
            {
                var actionName = string.Empty;
                //Get the query parameters       
                var queryParam = context.Request.Query["m"];
                var routeParams = path.Value.Split("/");
                var routeParam = routeParams.LastOrDefault();
                if (!string.IsNullOrEmpty(routeParam))
                {
                    actionName = routeParam;
                }
                else if (!string.IsNullOrEmpty(queryParam))
                {
                    actionName = queryParam;
                }
                else
                {
                    actionName = string.Empty;
                }
                if (!string.IsNullOrEmpty(actionName))
                {
                    var newBody =
                    $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soapenv:Envelope     
            	  xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                  xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
                <soapenv:Body>
                 <syb:ExecuteAction>
                   <syb:ActionName>{actionName}</syb:ActionName>
                 </syb:ExecuteAction>
                </soapenv:Body>
            </soapenv:Envelope>
            """;
                    context.Request.Body = newBody.ToStream();
                }
                else
                {
                    var newBody =
                    $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soapenv:Envelope     
            	  xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                  xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
                <soapenv:Body>
                 <syb:InvalidRequest></syb:InvalidRequest>
                </soapenv:Body>
            </soapenv:Envelope>
            """;
                    context.Request.Body = newBody.ToStream();
                }

            }
            return next(context);

        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.UseSoapEndpoint<ISoapService>(options =>
            {
                options.Path = "/soap";
                options.IndentXml = true;
                options.HttpGetEnabled = true;
                options.AdditionalEnvelopeXmlnsAttributes = new Dictionary<string, string>()
                {
                    { "syb", "http://sybrin.co.za/SoapSimulator.Core" },
                    { "core", "http://schemas.datacontract.org/2004/07/SoapSimulator.Core" },
                    { "array", "http://schemas.microsoft.com/2003/10/Serialization/Arrays" }
                };
            });

        });
        app.UseHangfireServer();
        app.MigrateDatabase();
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
