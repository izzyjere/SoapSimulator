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
        <soapenv:Envelope     
       	  xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
             xmlns:syb="http://sybrin.co.za/soap">
           <soapenv:Body>
               <syb:ExecuteAction>           
                   <syb:ActionName>{Example}</syb:ActionName>                   
               </syb:ExecuteAction>
           </soapenv:Body>
       </soapenv:Envelope>
       """;
    }

}
