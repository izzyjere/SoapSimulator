using System;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

public class ActionService : IActionService
{
    readonly DatabaseContext _db;
    readonly IWebHostEnvironment _env;
   
    public ActionService(DatabaseContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;        
    }

    public ActionResponse ExecuteAction(string actionId)
    {

        var action = _db.SoapActions.FirstOrDefault(x => x.Id == Guid.Parse(actionId));
        if (action == null)
        {
            ActionLogService.Log(nameof(ActionService), $"Action '{actionId}' not found.");
            throw new HttpRequestException($"404. Action '{actionId}' not found.");
        }
        ActionLogService.Log(nameof(ActionService), $"Executing action {actionId}");

        var result = action.Status switch
        {
            ActionStatus.Failure => throw new HttpRequestException("The action is set to fail."),            
            ActionStatus.Not_Found => throw new HttpRequestException($"Action/Method is not found."),           
            _ => ActionResponse.Success(action.GetResponse(action.Status).Body)

        };
        ActionLogService.Log(nameof(ActionService), $"Executed action {actionId} successfully.");
        return result;


    }
}
