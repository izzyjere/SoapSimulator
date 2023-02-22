namespace SoapSimulator.Core.Models;
[DataContract(Namespace = "http://soapsimulator/SoapSimulator.Core")]
public class ActionParameters
{
    [DataMember]
    public List<Parameter>? Parameters { get; set; }   
}
[DataContract(Namespace = "http://soapsimulator/SoapSimulator.Core")]
public class Parameter
{
    [SoapAttribute]
    public string Name { get; set; }
    [DataMember]
    public string Value { get; set; }
}