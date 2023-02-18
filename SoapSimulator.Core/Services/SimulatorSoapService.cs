using SoapSimulator.Core.Models;
namespace SoapSimulator.Core.Services;

public class SimulatorSoapService : ISoapService
{
    readonly ILogService logService;
    readonly IActionService actionService;
    
    public SimulatorSoapService(ILogService _logService, IActionService _actionService)
    {
        logService = _logService;
        actionService = _actionService;
    }

    public string Ping(string msg)
    {
        logService.Log(nameof(Ping), $"Ping from {msg}.");
        return "Hello world. " + msg;
    }

    public IActionResponse ExecuteAction(ActionParameter parameter)
    {
        logService.Log("", "Called.");
        return actionService.ExecuteAction(parameter.ActionName);
    }

}
