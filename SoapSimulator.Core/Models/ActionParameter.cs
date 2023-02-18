using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SoapSimulator.Core.Models;
[DataContract(Namespace = "http://sybrin.com/soap")]
public class ActionParameter
{
    [DataMember]
    public string ActionName { get; set; }
    [DataMember]
    public List<Parameter>? Parameters { get; set; }   
}
[DataContract(Namespace = "http://sybrin.com/soap")]
public class Parameter
{
    [SoapAttribute]
    public string Name { get; set; }
    [DataMember]
    public object Value { get; set; }
}