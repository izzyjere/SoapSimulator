using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public class ConfigurationService : IConfigurationService
{
    readonly DatabaseContext _db;
	public ConfigurationService(DatabaseContext db)
	{
		_db= db;
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

    public async Task<bool> UpdateConfigurationAsync(SystemConfiguration configuration)
    {
        _db.SystemConfigurations.Update(configuration);
        return await _db.SaveChangesAsync() != 0;
    }
}
