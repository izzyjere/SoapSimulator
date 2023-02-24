namespace SoapSimulator.Core.Models;
public class RequestFormat
{
    public Guid Id { get; set; }
    public string Body { get; set; } 
    public string XMLFileName { get; set; }
    public DateTime DateCreated { get; set; }
    public SoapAction Action { get; set; }
    public RequestFormat()
    {
        DateCreated = DateTime.Now;
        Body =
       """
       <?xml version="1.0" encoding="utf-8"?>
        <soapenv:Envelope     
          	     xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                 xmlns:soapcore="http://soapsimulator/SoapSimulator.Core"
                 xmlns:model="http://schemas.datacontract.org/2004/07/SoapSimulator.Core"
                 xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
              <soapenv:Body>
               <soapcore:ExecuteAction>
                 <soapcore:ActionId>{action.Id}</soapcore:ActionId>
                 <soapcore:RequestBody i:type="model:DynamicXmlObject">
                   
                 </soapcore:RequestBody>
               </soapcore:ExecuteAction>
              </soapenv:Body>
       </soapenv:Envelope>
       """;
    }

}
