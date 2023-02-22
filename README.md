# SoapSimulator
ASP.Core Server for Simulating SOAP Protocol. Only useful as a testing environment.
## General Usage
The current configuartion ignores any request validation this is mainly for cases where you just want to test various responses from actual soap services.
To change this behavious modify <code>UseSoapSimulatorCore</code> extension method inside <code>SoapSimulator.Core.Extensions.cs</code> file.
```csharp
app.Use((context,next) =>
{
    
    var path = context.Request.Path;
    if (path.HasValue && path.StartsWithSegments("/soap")&& !context.Request.Query.ContainsKey("WSDL"))
    {
        var actionName = string.Empty;                
        var queryParam = string.Empty;
        var routeParam = string.Empty;

        var queryParams = context.Request.Query;
        if (queryParams.Any() ) 
        {   
            
            if (queryParams.ContainsKey("m"))
            {
                queryParam = queryParams["m"];
            }
            else if (queryParams.ContainsKey("method"))
            {
                queryParam = queryParams["method"];
            }
            else
            {
                throw new HttpRequestException("Invalid query parameter use 'm' or 'method' ");
            } 
        }
        var routeParams = path.Value.Split("/");
        if(routeParams.Length == 3)
        {
            routeParam = routeParams.LastOrDefault();
        }
        else
        {
            routeParam = string.Empty;
        }
        if (!string.IsNullOrEmpty(queryParam))
        {
            actionName = queryParam;
        }
        else if (!string.IsNullOrEmpty(routeParam))
        {
            actionName = routeParam;
        }                
        else
        {
            actionName = string.Empty;
        }
        if (string.IsNullOrEmpty(actionName))
        {   
            //Disable this behaviour
            var newBody =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soapenv:Envelope     
                	 xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                     xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
               <soapenv:Body>
                 <syb:InvalidRequest></syb:InvalidRequest>
               </soapenv:Body>
            </soapenv:Envelope>
            """;
            context.Request.Body = newBody.ToStream();
            context.Request.Path = "/soap";
            return next(context);
        }
        else
        {
            var db = new DatabaseContext();
            var action =( db.SoapActions).FirstOrDefault(a => a.MethodName.ToLower() == actionName.ToLower());
            if(action==null)
            {
                var newBody =
                 $"""
                   <?xml version="1.0" encoding="utf-8"?>
                   <soapenv:Envelope     
                       	 xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                            xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
                      <soapenv:Body>
                        <syb:MethodNotFound><syb:name>{actionName}</syb:name></syb:MethodNotFound>
                      </soapenv:Body>
                   </soapenv:Envelope>
                   """;
                context.Request.Body = newBody.ToStream();
                context.Request.Path = "/soap";
                return next(context);
            }
            else if(action.Status == ActionStatus.No_Response)
            {
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                context.Response.ContentLength = 0;
                return Task.CompletedTask;
            }
            else
            {
                //Disable this behaviour
                var newBody =
                $"""
                <?xml version="1.0" encoding="utf-8"?>
                 <soapenv:Envelope     
                   	      xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                          xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
                       <soapenv:Body>
                        <syb:ExecuteAction>
                          <syb:ActionId>{action.Id}</syb:ActionId>
                        </syb:ExecuteAction>
                       </soapenv:Body>
                </soapenv:Envelope>
                """;
                context.Request.Body = newBody.ToStream();
                context.Request.Path = "/soap";
                return next(context);
            }
        }

    }
    else
    {
        return next(context);
    }
    
});
```
