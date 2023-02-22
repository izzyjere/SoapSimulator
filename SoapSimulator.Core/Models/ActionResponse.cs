namespace SoapSimulator.Core.Models;

[DataContract(Namespace = "http://soapsimulator/SoapSimulator.Core")]
[KnownType(typeof(DynamicXmlObject))]
public class ActionResponse
{

    [DataMember]
    public string Method { get; private set; }
    [DataMember(Name = "Data")]
    public dynamic Content { get; set; }

    [DataMember]
    public string Status { get; private set; }

    [DataMember]
    public string Message { get; private set; }

    [OperationContract]
    public static ActionResponse Success(DynamicObject content,string method, string message = "")
    {
        return new ActionResponse { Content =  content, Status = "Success",Method=method, Message = message };
    }

    [OperationContract]
    public static ActionResponse Failure(string message, string method)
    {
        return new ActionResponse { Status = "Failure",Method=method, Message = message };
    }
}

