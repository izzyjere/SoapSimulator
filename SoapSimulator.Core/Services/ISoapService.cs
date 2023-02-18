using System.ServiceModel;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
[ServiceContract(Namespace = "http://sybrin.co.za/soap")]
public interface ISoapService
{
    [OperationContract]
    ActionResponse ExecuteAction(string ActionName);
    [OperationContract]
    string Ping(string msg);
}