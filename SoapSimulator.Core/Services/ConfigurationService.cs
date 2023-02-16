using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public class ConfigurationService : IConfigurationService
{
    readonly DatabaseContext _db;
	public ConfigurationService(DatabaseContext db)
	{
		_db= db;
	}

    public Task DeleteConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public Task<SystemConfiguration> GetCurrentConfigurationAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public Task SetCurrentConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public Task UpdateConfigurationAsync(SystemConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
