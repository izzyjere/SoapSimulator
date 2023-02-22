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
                     xmlns:core="http://corerin.co.za/SoapSimulator.Core">
               <soapenv:Body>
                 <core:InvalidRequest></core:InvalidRequest>
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
                            xmlns:core="http://corerin.co.za/SoapSimulator.Core">
                      <soapenv:Body>
                        <core:MethodNotFound><core:name>{actionName}</core:name></core:MethodNotFound>
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
                          xmlns:core="http://corerin.co.za/SoapSimulator.Core">
                       <soapenv:Body>
                        <core:ExecuteAction>
                          <core:ActionId>{action.Id}</core:ActionId>
                        </core:ExecuteAction>
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
## Sample Response from the server for the soap request <code></code>
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:core="http://soapserver/SoapSimulator.Core" xmlns:core="http://schemas.datacontract.org/2004/07/SoapSimulator.Core" xmlns:array="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
    <s:Body>
        <core:ExecuteActionResponse>
            <core:ExecuteActionResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
                <core:Body i:type="core:DynamicXmlObject">
                    <Transaction>
                        <Transaction>
                            <Date>2022-02-16T17:30:45</Date>
                            <CreditValueDate>2022-02-16T17:30:45</CreditValueDate>
                            <DebitValueDate>2022-02-16T17:30:45</DebitValueDate>
                            <CreditAmount>250.00</CreditAmount>
                            <DebitAccount>10010002</DebitAccount>
                            <CreditAccount>10010003</CreditAccount>
                            <Narration>Transfer to Savings</Narration>
                            <DestinationBankSortCode>BR003</DestinationBankSortCode>
                        </Transaction>
                        <Transaction>
                            <Date>2022-02-17T14:20:10</Date>
                            <CreditValueDate>2022-02-17T14:20:10</CreditValueDate>
                            <DebitValueDate>2022-02-17T14:20:10</DebitValueDate>
                            <CreditAmount>120.00</CreditAmount>
                            <DebitAccount>10010002</DebitAccount>
                            <CreditAccount>10010001</CreditAccount>
                            <Narration>ATM Withdrawal</Narration>
                            <DestinationBankSortCode>BR001</DestinationBankSortCode>
                        </Transaction>
                        <Transaction>
                            <Date>2022-02-18T09:12:34</Date>
                            <CreditValueDate>2022-02-18T09:12:34</CreditValueDate>
                            <DebitValueDate>2022-02-18T09:12:34</DebitValueDate>
                            <CreditAmount>2500.00</CreditAmount>
                            <DebitAccount>10010002</DebitAccount>
                            <CreditAccount>10010001</CreditAccount>
                            <Narration>Salary Payment</Narration>
                            <DestinationBankSortCode>BR001</DestinationBankSortCode>
                        </Transaction>
                    </Transaction>
                </core:Body>
                <core:Message></core:Message>
                <core:Status>Success</core:Status>
            </core:ExecuteActionResult>
        </core:ExecuteActionResponse>
    </s:Body>
</s:Envelope>
```