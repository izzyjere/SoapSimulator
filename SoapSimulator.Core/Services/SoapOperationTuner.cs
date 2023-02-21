namespace SoapSimulator.Core.Services;
public class SoapOperationTuner : IServiceOperationTuner
{
    public  void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
    {
        if (operation.Name.Equals("ExecuteAction"))
        {
            StringValues paramValue;
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out paramValue))
            {
                var userAgent = paramValue[0];
                ActionLogService.Log(nameof(SoapOperationTuner),$"Execute action request from {userAgent}");
            }

        } 
        if (operation.Name.Equals("Ping"))
        {
            StringValues paramValue;
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out paramValue))
            {
                var userAgent = paramValue[0];
                ActionLogService.Log(nameof(SoapOperationTuner),$"Ping request from {userAgent}");
            }

        }

    }
   
}
