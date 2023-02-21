namespace SoapSimulator.Core.Services;

public class SimulatorSoapService : ISoapService
{
    
    readonly IActionService actionService;
    
    public SimulatorSoapService(IActionService _actionService)
    {
        actionService = _actionService;
        ActionLogService.Log(nameof(SimulatorSoapService), "Soap service initialized.");
    }

    public string Ping(string Msg="")
    {
       return "Hello world. " + Msg;
    }

    public ActionResponse ExecuteAction(string ActionName, ActionParameters? ActionParameters)
    { 
        if(string.IsNullOrEmpty(ActionName))
        {
            ActionLogService.Log(nameof(ExecuteAction), "No action name was specified.");
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

    public string InvalidRequest()
    {
        return "No action was specified. use 'https://hostname/soap/{methodName} or https://hostname/soap?m={methodName}'  parameters to provide the action name.";
    }
}
