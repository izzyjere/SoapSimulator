﻿namespace SoapSimulator.Core.Services;

public class ActionService : IActionService
{
    readonly DatabaseContext _db;
    readonly IWebHostEnvironment _env;
    readonly ILogService logService;
    public ActionService(DatabaseContext db, IWebHostEnvironment env, ILogService logService)
    {
        _db = db;
        _env = env;
        this.logService = logService;
    }

    public ActionResponse? ExecuteAction(string actionName)
    {

        var action = _db.SoapActions.FirstOrDefault(x => x.MethodName == actionName);
        if (action == null)
        {
            logService.Log(nameof(ActionService), $"Action '{actionName}' not found.");
            throw new HttpRequestException($"404. Action '{actionName}' not found.");
        }
        logService.Log(nameof(ActionService), $"Executing action {actionName}");

        var result = action.Status switch
        {
            ActionStatus.Failure => null,
            ActionStatus.Not_Found => throw new HttpRequestException($"404. Action '{actionName}' not found."),
            ActionStatus.No_Response => throw new HttpProtocolException(204, "204 No response", new Exception("Action is currently not accepting requests.")),
            _ => ActionResponse.Success(action.GetResponse(action.Status).Body)

        };
        logService.Log(nameof(ActionService), $"Executed action {actionName} successfully.");
        return result;


    }
}
