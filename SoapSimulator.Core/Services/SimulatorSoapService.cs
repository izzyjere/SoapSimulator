using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

public class SimulatorSoapService : ISoapService
{
    readonly ILogService logService;
    public SimulatorSoapService(ILogService logService)
    {
        this.logService = logService;
    }

    public Task<IActionResponse> ExecuteAction(Guid actionId)
    {
        throw new NotImplementedException();
    }

    public Task<string> Ping()
    {
       logService.Log(nameof(Ping),"Service Called.");
       return Task.FromResult("Hello world.");
    }

    public Task SetActionParameter(Guid actionId, ActionParameter parameter)
    {
        throw new NotImplementedException();
    }
}
