using System.Buffers;
using System.IO.Pipelines;
using System.Text;

using Microsoft.AspNetCore.Http;

using SoapCore.Extensibility;
using SoapCore.ServiceModel;

namespace SoapSimulator.Core.Services;
public class SoapOperationTuner : IServiceOperationTuner
{
    public  void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
    {
        if (operation.Name.Equals("ExecuteAction"))
        {
            var service = serviceInstance as ISoapService;
           
        }

    }
   
}
