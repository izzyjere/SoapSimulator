namespace SoapSimulator.Core.Services;
[ServiceContract(Namespace = "http://soapsimulator/SoapSimulator.Core")]
public interface ISoapService
{
    [OperationContract] 
    ActionResponse ExecuteAction(string ActionId, DynamicXmlObject RequestBody);
    [OperationContract]
    string Ping(string Msg="");
    [OperationContract]
    string InvalidRequest();
    string MethodNotFound(string name);
}