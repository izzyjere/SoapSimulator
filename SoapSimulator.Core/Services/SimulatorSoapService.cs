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
    public ActionResponse ExecuteAction(string ActionId, DynamicXmlObject RequestBody)
    { 
        if(string.IsNullOrEmpty(ActionId))
        {
            ActionLogService.Log(nameof(ExecuteAction), "No action name was specified.");
            throw new HttpRequestException("No action was specified. Use element 'ActionId' inside 'ExecuteAction' to specify a service action.");
        }
        //if(RequestBody== null)
        //{
        //    throw new HttpRequestException("No action request body was provided. Use element 'RequestBody' inside 'ExecuteAction' to specify a service action.");
        //}
        var response = actionService.ExecuteAction(ActionId);       
        return response;
    }

    public string InvalidRequest()
    {
        return "No action was specified. use 'https://hostname/soap/{methodName} or https://hostname/soap?m={methodName}'  parameters to provide the action name.";
    }
}
