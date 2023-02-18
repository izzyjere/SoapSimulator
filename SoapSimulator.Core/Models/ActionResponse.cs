using System.Runtime.Serialization;
using System.ServiceModel;

namespace SoapSimulator.Core.Models;

[DataContract (Namespace ="http://sybrin.com/soap")]
public class ActionResponse 
{
    [DataMember]
    public string Body { get; set; }
    [DataMember]
    public string Status { get; private set; }
    [DataMember]
    public string Message { get; private set; }
    [OperationContract]
    public static ActionResponse Success(string body, string message="")
    {
        return new ActionResponse { Body = body, Status = "Success", Message = message };
    }
    [OperationContract]
    public static ActionResponse Failure(string message)
    {
        return new ActionResponse { Status = "Failure" , Message = message };
    }
}

