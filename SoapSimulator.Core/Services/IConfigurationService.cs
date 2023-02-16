using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

public interface IConfigurationService
{
    Task<SystemConfiguration> GetCurrentConfigurationAsync();
    Task SaveConfigurationAsync(SystemConfiguration configuration);
    Task DeleteConfigurationAsync(SystemConfiguration configuration);
    Task UpdateConfigurationAsync(SystemConfiguration configuration);
    Task SetCurrentConfigurationAsync(SystemConfiguration configuration);
}