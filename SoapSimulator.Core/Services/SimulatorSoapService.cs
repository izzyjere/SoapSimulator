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

    public ActionResponse ExecuteAction(string ActionName, ActionParameters? ActionParameters)
    { 
        if(string.IsNullOrEmpty(ActionName))
        {
            logService.Log(nameof(ExecuteAction), "No action name was specified.");
            throw new HttpRequestException("No action was specified. Use element 'ActionName' inside 'ExecuteAction' to specify a service action.");
        }
        if(ActionName=="Ping")
        {
            return ActionResponse.Success("<string>Hello World.</string>");
        }
        var response = actionService.ExecuteAction(ActionName);
        if (response == null)
        {
            throw new HttpRequestException($"Action {ActionName} is set fail.");
        }
        return response;
    }

}
