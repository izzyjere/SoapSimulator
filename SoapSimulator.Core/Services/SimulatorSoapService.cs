using System.ServiceModel;
using SoapSimulator.Core.Models;
namespace SoapSimulator.Core.Services;

public class SimulatorSoapService : ISoapService
{
    readonly ILogService logService;
    public SimulatorSoapService(ILogService logService)
    {
        this.logService = logService;
    }

    public string Ping(string msg)
    {
        logService.Log(nameof(Ping), $"Ping from {msg}.");
        return "Hello world. " + msg;
    }

    public IActionResponse ExecuteAction(string actionName)
    {
        logService.Log(actionName, "Called.");
        return new ActionResponse() { Body = "Test" + actionName };
    }

    public void SetActionParameter(Guid actionId, ActionParameter parameter)
    {
        throw new NotImplementedException();
    }
}
