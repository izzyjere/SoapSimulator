using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public class ConfigurationService : IConfigurationService
{
    readonly DatabaseContext _db;
    readonly IWebHostEnvironment _environment;
	public ConfigurationService(DatabaseContext db, IWebHostEnvironment environment)
	{
		_db= db;
        _environment = environment;
	}

    public async Task<bool> DeleteConfigurationAsync(SystemConfiguration configuration)
    {
        configuration.Actions.ForEach(async action =>
        {
            await DeleteXSD(action.Request.XMLPath);
            await DeleteXSD(action.Response.XMLPath);
        });
        _db.SystemConfigurations.Remove(configuration);
        return await _db.SaveChangesAsync() !=0;
    }

    public async Task<SystemConfiguration> GetCurrentConfigurationAsync()
    {
        return await _db.SystemConfigurations
                        .Include(c=>c.Actions)
                        .FirstOrDefaultAsync(c=>c.IsCurrent);
    }

    public async Task<IEnumerable<SystemConfiguration>> GetAllConfigurationsAsync()
    {
        return await _db.SystemConfigurations.Include(c => c.Actions).ToListAsync();
    }

    public async Task<bool> SaveConfigurationAsync(SystemConfiguration configuration)
    {
        configuration.Actions.ForEach(async action =>
        {
            action.Response.XMLPath = await SaveXSD(action.Response.Body);
            action.Request.XMLPath = await SaveXSD(action.Request.Body);
        });
        _db.SystemConfigurations.Add(configuration);
        return await _db.SaveChangesAsync()!=0;
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
        if(File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }
    private Task<string> SaveXSD(string xsd)
    {   
        var fileName = Path.GetRandomFileName().Replace(".","_") + ".xml";
        var path = Path.Combine(_environment.WebRootPath, "xml", fileName); 
        File.WriteAllText(path, xsd);
        return Task.FromResult(fileName);
    }
    public async Task<bool> UpdateConfigurationAsync(SystemConfiguration configuration)
    {
        _db.SystemConfigurations.Update(configuration);
        return await _db.SaveChangesAsync() != 0;
    }
}
