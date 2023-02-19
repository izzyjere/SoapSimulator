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

    public string Ping(string Msg="")
    {
       return "Hello world. " + Msg;
    }

    public ActionResponse ExecuteAction(string ActionName)
    { 
       var response = actionService.ExecuteAction(ActionName);
        if (response == null)
        {
            throw new HttpRequestException($"Action {ActionName} is set fail.");
        }
        return response;
    }

}
