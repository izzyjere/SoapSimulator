using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

public interface ISoapService
{
    Task SetActionParameter(Guid actionId, ActionParameter parameter);  
    Task<IActionResponse> ExecuteAction(Guid actionId);
    Task<string> Ping() => Task.FromResult("Hello world.");
}