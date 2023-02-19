﻿namespace SoapSimulator.Core.Models;
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
             xmlns:syb="http://sybrin.co.za/SoapSimulator.Core">
           <soapenv:Body>
               <syb:ExecuteAction>           
                   <syb:ActionName>{ExampleAction}</syb:ActionName>                   
               </syb:ExecuteAction>
           </soapenv:Body>
       </soapenv:Envelope>
       """;
    }

}
