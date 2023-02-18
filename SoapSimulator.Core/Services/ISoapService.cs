using System.Runtime.Serialization;
using System.ServiceModel;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
[ServiceContract(Namespace = "http://sybrin.co.za/soap")]
public interface ISoapService
{
    [OperationContract]
    IActionResponse ExecuteAction(ActionParameter parameter);
    [OperationContract]
    string Ping(string msg);
}