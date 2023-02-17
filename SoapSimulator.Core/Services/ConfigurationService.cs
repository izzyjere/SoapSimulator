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
        _db.SystemConfigurations.Remove(configuration);
        return await _db.SaveChangesAsync() !=0;
    }

    public async Task<SystemConfiguration> GetCurrentConfigurationAsync()
    {
        return await _db.SystemConfigurations
                        .Include(c=>c.Actions)
                        .ThenInclude(a=>a.Parameters)
                        .FirstOrDefaultAsync(c=>c.IsCurrent);
    }

    public async Task<IEnumerable<SystemConfiguration>> GetAllConfigurationsAsync()
    {
        return await _db.SystemConfigurations.Include(c => c.Actions).ThenInclude(a => a.Parameters).ToListAsync();
    }

    public async Task<bool> SaveConfigurationAsync(SystemConfiguration configuration)
    {
        configuration.Actions.ForEach(async action =>
        {
            action.ResponseFormat.XSDPath = await SaveXSD(action.ResponseFormat.Body);
            action.RequestFormat.XSDPath = await SaveXSD(action.RequestFormat.Body);
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
    private Task<string> SaveXSD(string xsd)
    {   
        var fileName = Path.GetRandomFileName().Replace(".","_") + ".xsd";
        var path = Path.Combine(_environment.WebRootPath, "xsd", fileName); 
        File.WriteAllText(path, xsd);
        return Task.FromResult(fileName);
    }
    public async Task<bool> UpdateConfigurationAsync(SystemConfiguration configuration)
    {
        _db.SystemConfigurations.Update(configuration);
        return await _db.SaveChangesAsync() != 0;
    }
}
