﻿namespace SoapSimulator.Core.Models;

[DataContract(Namespace = "http://sybrin.co.za/SoapSimulator.Core")]
[KnownType(typeof(DynamicXmlObject))]
public class ActionResponse
{
    [DataMember(Name = "Body")]
    public dynamic Content { get; set; }

    [DataMember]
    public string Status { get; private set; }

    [DataMember]
    public string Message { get; private set; }

    [OperationContract]
    public static ActionResponse Success(DynamicObject content, string message = "")
    {
        return new ActionResponse { Content =  content, Status = "Success", Message = message };
    }

    [OperationContract]
    public static ActionResponse Failure(string message)
    {
        return new ActionResponse { Status = "Failure", Message = message };
    }
}

