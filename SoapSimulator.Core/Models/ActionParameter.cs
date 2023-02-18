using System.Runtime.Serialization;

namespace SoapSimulator.Core.Models;

[DataContract]
public class ActionParameter
{
    [DataMember]
    public string ActionName { get; set; }
    [DataMember]
    public List<Parameter> Parameters { get; set; }   
}
[DataContract]
public class Parameter
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public object Value { get; set; }
}