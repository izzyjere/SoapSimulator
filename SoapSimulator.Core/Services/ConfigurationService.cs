using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public class ConfigurationService
{
    readonly DatabaseContext _db;
	public ConfigurationService(DatabaseContext db)
	{
		_db= db;
	}
	public async Task<SystemConfiguration> GetCurrentConfigurationAsync()
	{
      
	}
}
