using Microsoft.AspNetCore.Hosting.StaticWebAssets;

using MudBlazor.Services;

using SoapCore;

using SoapSimulator.Core;
using SoapSimulator.Core.Services;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
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
app.UseEndpoints(endpoints =>
{
    endpoints.UseSoapEndpoint<ISoapService>(options =>
    {
        options.Path = "/soap";
        options.IndentXml = true;      
        options.AdditionalEnvelopeXmlnsAttributes = new Dictionary<string, string>()
        {
             { "syb", "http://sybrin.com/soap" },
             { "array", "http://schemas.microsoft.com/2003/10/Serialization/Arrays" },
             { "xsi","http://www.w3.org/2001/XMLSchema-instance" },
             { "xsd","http://www.w3.org/2001/XMLSchema" }
        };
    });

});
app.MigrateDatabase();
app.Run();