using System.Runtime.Serialization;

namespace SoapSimulator.Core.Models;
[DataContract]
public class ActionRequest : IActionRequest
{
    [DataMember]
    public string Body { get; set; }
}
public interface IActionRequest
{
    string Body { get; set; }
}
