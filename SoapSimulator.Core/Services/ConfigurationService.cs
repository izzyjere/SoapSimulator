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

    public Task<IEnumerable<SystemConfiguration>> GetAllConfigurationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetCurrentConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
