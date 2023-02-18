﻿using Microsoft.AspNetCore.Hosting;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

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

    public IActionResponse ExecuteAction(string actionName)
    {
        var action = _db.SoapActions.FirstOrDefault(x => x.MethodName== actionName);
        if(action == null)
        {
            logService.Log(nameof(ActionService), $"Action '{actionName}' not found.");
            return ActionResponse.Failure("Action not found.");
        }
        logService.Log(nameof(ActionService),$"Executing action {actionName}");

        return action.Status switch
        {
            ActionStatus.Success => ActionResponse.Success(action.Response.Body),
            ActionStatus.Failure => throw new Exception("Action is set to fail"),
            ActionStatus.NoResponse => ActionResponse.Success("","No records found."),
            ActionStatus.NotFound => ActionResponse.Failure("Action not found."),
            _ => ActionResponse.Success(action.Response.Body)

        };    
    }
}
