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
        app.Use((httpContext,middleware) =>
        {
            
            var path = httpContext.Request.Path;
            if (path.HasValue && path.StartsWithSegments("/soap")&& !httpContext.Request.Query.ContainsKey("WSDL"))
            {
                var actionName = string.Empty;                
                var queryParam = string.Empty;
                var routeParam = string.Empty;

                var queryParams = httpContext.Request.Query;
                if (queryParams.Any() ) 
                {   
                    
                    if (queryParams.ContainsKey("m"))
                    {
                        queryParam = queryParams["m"];
                    }
                    else if (queryParams.ContainsKey("method"))
                    {
                        queryParam = queryParams["method"];
                    }
                    else
                    {
                        throw new HttpRequestException("Invalid query parameter use 'm' or 'method' ");
                    } 
                }
                var routeParams = path.Value.Split("/");
                if(routeParams.Length == 3)
                {
                    routeParam = routeParams.LastOrDefault();
                }
                else
                {
                    routeParam = string.Empty;
                }
                if (!string.IsNullOrEmpty(queryParam))
                {
                    actionName = queryParam;
                }
                else if (!string.IsNullOrEmpty(routeParam))
                {
                    actionName = routeParam;
                }                
                else
                {
                    actionName = string.Empty;
                }
                if (string.IsNullOrEmpty(actionName))
                {
                    var newBody =
                    $"""
                    <?xml version="1.0" encoding="utf-8"?>
                    <soapenv:Envelope     
                        	 xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                             xmlns:soapcore="http://soapsimulator/SoapSimulator.Core">
                       <soapenv:Body>
                         <soapcore:InvalidRequest></soapcore:InvalidRequest>
                       </soapenv:Body>
                    </soapenv:Envelope>
                    """;
                    httpContext.Request.Body = newBody.ToStream();
                    httpContext.Request.Path = "/soap";
                    return middleware(httpContext);
                }
                else
                {
                    var db = new DatabaseContext();
                    var action =( db.SoapActions).FirstOrDefault(a => a.MethodName.ToLower() == actionName.ToLower());
                    if(action==null)
                    {
                        var newBody =
                         $"""
                           <?xml version="1.0" encoding="utf-8"?>
                           <soapenv:Envelope     
                               	 xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                                    xmlns:soapcore="http://soapsimulator/SoapSimulator.Core">
                              <soapenv:Body>
                                <soapcore:MethodNotFound><soapcore:name>{actionName}</soapcore:name></soapcore:MethodNotFound>
                              </soapenv:Body>
                           </soapenv:Envelope>
                           """;
                        httpContext.Request.Body = newBody.ToStream();
                        httpContext.Request.Path = "/soap";
                        return middleware(httpContext);
                    }
                    else if(action.Status == ActionStatus.No_Response)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
                        httpContext.Response.ContentLength = 0;
                        return Task.CompletedTask;
                    }
                    else
                    {
                        var newBody =
                        $"""
                        <?xml version="1.0" encoding="utf-8"?>
                         <soapenv:Envelope     
                           	      xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                                  xmlns:soapcore="http://soapsimulator/SoapSimulator.Core">
                               <soapenv:Body>
                                <soapcore:ExecuteAction>
                                  <soapcore:ActionId>{action.Id}</soapcore:ActionId>
                                </soapcore:ExecuteAction>
                               </soapenv:Body>
                        </soapenv:Envelope>
                        """;
                        httpContext.Request.Body = newBody.ToStream();
                        httpContext.Request.Path = "/soap";
                        return middleware(httpContext);
                    }
                }

            }
            else
            {
                return middleware(httpContext);
            }
            
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
                    { "soapcore", "http://soapsimulator/SoapSimulator.Core" },
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
