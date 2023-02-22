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
            throw new HttpRequestException($"An error ha occured.");
        }
        ActionLogService.Log(nameof(ActionService), $"Executing action {actionId}");
        if(action.Status == ActionStatus.Not_Found)
        {
            throw new HttpRequestException($"Action/Method is not found.");
        }
        if(action.Status == ActionStatus.Failure)
        {
            throw new HttpRequestException("The action is set to fail.");
        }
        var response = action.GetResponse(action.Status);
        if(response == null)
        {
            throw new HttpRequestException("Something went wrong while processing your request.");
        }
        var xmlFilePath = Path.Combine(_env.WebRootPath, "xml", response.XMLFileName);
        var xml = File.ReadAllText(xmlFilePath);
        var result = ActionResponse.Success(DynamicXmlObject.Deserialize(xml));
        ActionLogService.Log(nameof(ActionService), $"Executed action {actionId} successfully.");
        return result;
    }
}
