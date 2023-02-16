using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;

using SoapCore;

using SoapSimulator;
using SoapSimulator.Core;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSoapSimulatorCore();
builder.Services.AddMudServices();
builder.Services.AddSoapCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.UseSoapEndpoint<SimulatorSoapService>("/soapsimulator.asmx", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
});
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();