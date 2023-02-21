using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Newtonsoft.Json;
using SoapCore;
using SoapSimulator.Core;
using SoapSimulator.Core.Services;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHangfire(configuration => configuration
            .UseSerializerSettings(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            })
            .UseMemoryStorage());
builder.Services.AddSoapSimulatorCore();
builder.Services.AddMudServices();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowAnyOrigin();
});
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Use((context,next) =>
{
    if(context.Request.Path=="/soap")
    {   
        //Get the query parameters       
        var actionName = context.Request.Query["actionName"];
        if(!string.IsNullOrEmpty(actionName))
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
                 <syb:NoAction>
                   
                 </syb:NoAction>
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
app.Run();