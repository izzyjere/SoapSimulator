using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

public interface IConfigurationService
{
    Task<SystemConfiguration> GetCurrentConfigurationAsync();
    Task<bool> SaveConfigurationAsync(SystemConfiguration configuration);
    Task<bool> DeleteConfigurationAsync(SystemConfiguration configuration);
    Task<bool> UpdateConfigurationAsync(SystemConfiguration configuration);
    Task<bool> SetCurrentConfigurationAsync(SystemConfiguration configuration);
    Task<IEnumerable<SystemConfiguration>> GetAllConfigurationsAsync();
    Task DeleteXSD(string fileName);
    Task UpdateActionStatus(Guid actionId, ActionStatus status);
}