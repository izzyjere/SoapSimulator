using System.Runtime.Serialization;

namespace SoapSimulator.Core.Models;

[DataContract]
public class ActionResponse : IActionResponse
{
    [DataMember]
    public string Body { get; set; }
    [DataMember]
    public string Status { get; private set; }
    [DataMember]
    public string Message { get; private set; }

    public static IActionResponse Success(string body, string message="")
    {
        return new ActionResponse { Body = body, Status = "Success", Message = message };
    }
    public static IActionResponse Failure(string message)
    {
        return new ActionResponse { Status = "Failure" , Message = message };
    }
}
public interface IActionResponse
{
    string Body { get; set; }
    string Status { get; }
    string Message { get; }
}
