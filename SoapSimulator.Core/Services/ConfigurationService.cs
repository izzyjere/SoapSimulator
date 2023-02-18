using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public class ConfigurationService : IConfigurationService
{
    readonly DatabaseContext _db;
    readonly IWebHostEnvironment _environment;
    readonly ILogService logService;
    public ConfigurationService(DatabaseContext db, IWebHostEnvironment environment, ILogService logService)
    {
        _db = db;
        _environment = environment;
        this.logService = logService;
    }

    public async Task<bool> DeleteConfigurationAsync(SystemConfiguration configuration)
    {
        configuration.Actions.ForEach(async action =>
        {
            await DeleteXSD(action.Request.XMLFileName);
            await DeleteXSD(action.Response.XMLFileName);
        });
        _db.SystemConfigurations.Remove(configuration);
        return await _db.SaveChangesAsync() != 0;
    }
    public async Task UpdateActionAsync(SoapAction action)   
    {
       
        if (action != null)
        {
            action.Response.XMLFileName = await SaveXSD(action.Response.Body);
            action.Request.XMLFileName = await SaveXSD(action.Request.Body);
            _db.SoapActions.Update(action);
            await _db.SaveChangesAsync();
            logService.Log(nameof(ConfigurationService), $"Action {action.MethodName} updated.");
        }

    }

    public async Task<SystemConfiguration> GetCurrentConfigurationAsync()
    {
        return await _db.SystemConfigurations
                        .Include(c => c.Actions)
                        .FirstOrDefaultAsync(c => c.IsCurrent);
    }

    public async Task<IEnumerable<SystemConfiguration>> GetAllConfigurationsAsync()
    {
        return await _db.SystemConfigurations.Include(c => c.Actions).ToListAsync();
    }

    public async Task<bool> SaveConfigurationAsync(SystemConfiguration configuration)
    {
        if (configuration.IsCurrent)
        {
            foreach (var item in _db.SystemConfigurations.Where(c => c.IsCurrent))
            {
                item.IsCurrent = false;
                _db.SystemConfigurations.Update(item);
            }
        }
        else { }
        configuration.Actions.ForEach(async action =>
        {
            action.Response.XMLFileName = await SaveXSD(action.Response.Body);
            action.Request.XMLFileName = await SaveXSD(action.Request.Body);
        });
        _db.SystemConfigurations.Add(configuration);
        return await _db.SaveChangesAsync() != 0;
    }

    public async Task<bool> SetCurrentConfigurationAsync(SystemConfiguration configuration)
    {
        var currentConfig = await GetCurrentConfigurationAsync();
        if (currentConfig != null)
        {
            currentConfig.IsCurrent = false;
            _db.SystemConfigurations.Update(currentConfig);
            await _db.SaveChangesAsync();
        }
        else { }
        configuration.IsCurrent = true;
        return await UpdateConfigurationAsync(configuration);
    }
    public Task DeleteXSD(string fileName)
    {
        var path = Path.Combine(_environment.WebRootPath, "xml", fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }
    private Task<string> SaveXSD(string xsd)
    {
        var fileName = Path.GetRandomFileName().Replace(".", "_") + ".xml";
        var path = Path.Combine(_environment.WebRootPath, "xml", fileName);
        File.WriteAllText(path, xsd);
        return Task.FromResult(fileName);
    }
    public async Task<bool> UpdateConfigurationAsync(SystemConfiguration configuration)
    {
        if(configuration.IsCurrent)
        {
            foreach (var item in _db.SystemConfigurations.Where(c=>c.IsCurrent && c.Id!=configuration.Id))
            {
                item.IsCurrent = false;
               _db.SystemConfigurations.Update(item);
            }
        }
        else { }
        _db.SystemConfigurations.Update(configuration);
        return await _db.SaveChangesAsync() != 0;
    }
}
