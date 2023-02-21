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
    public string MethodNotFound(string name)
    {
        return $"Action/Method {name} was not found.";
    }
    public ActionResponse ExecuteAction(string ActionId, ActionParameters? ActionParameters)
    { 
        if(string.IsNullOrEmpty(ActionId))
        {
            ActionLogService.Log(nameof(ExecuteAction), "No action name was specified.");
            throw new HttpRequestException("No action was specified. Use element 'ActionName' inside 'ExecuteAction' to specify a service action.");
        }       
        var response = actionService.ExecuteAction(ActionId);       
        return response;
    }

    public string InvalidRequest()
    {
        return "No action was specified. use 'https://hostname/soap/{methodName} or https://hostname/soap?m={methodName}'  parameters to provide the action name.";
    }
}
