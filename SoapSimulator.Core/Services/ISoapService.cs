﻿using System.ServiceModel;

using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
[ServiceContract(Namespace = "http://sybrin.co.za/SoapSimulator.Core")]
public interface ISoapService
{
    [OperationContract]
    ActionResponse ExecuteAction(string ActionName, ActionParameters? ActionParameters);
    [OperationContract]
    string Ping(string Msg="");
}