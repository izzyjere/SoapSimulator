using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using SoapCore.Extensibility;
using SoapCore.ServiceModel;

namespace SoapSimulator.Core.Services;
public class SoapOperationTuner : IServiceOperationTuner
{
    readonly ILogService logService;
    public SoapOperationTuner(ILogService logService)
    {
        this.logService = logService;
    }

    public  void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
    {
        if (operation.Name.Equals("ExecuteAction"))
        {
            StringValues paramValue;
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out paramValue))
            {
                var userAgent = paramValue[0];
                logService.Log(nameof(SoapOperationTuner),$"New request from {userAgent}");
            }

        }

    }
   
}
