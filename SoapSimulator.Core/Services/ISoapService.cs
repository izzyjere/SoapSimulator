using System.Runtime.Serialization;
using System.ServiceModel;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
[ServiceContract(Namespace = "http://sybrin.co.za/soap")]
public interface ISoapService
{
    [OperationContract]
    void SetActionParameter(Guid actionId, ActionParameter parameter);
    [OperationContract]
    IActionResponse ExecuteAction(string actionName);
    [OperationContract]
    string Ping(string msg);
}