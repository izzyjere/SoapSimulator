using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;

public class SimulatorSoapService : ISoapService
{
    public Task<IActionResponse> ExecuteAction(Guid actionId)
    {
        throw new NotImplementedException();
    }

    public Task SetActionParameter(Guid actionId, ActionParameter parameter)
    {
        throw new NotImplementedException();
    }
}
