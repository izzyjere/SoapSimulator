using SoapSimulator.Core.Models;

namespace SoapSimulator;

public class SimulatorSoapService
{
}
public interface ISoapService
{
    Task SetActionParameter(Guid actionId, ActionParameter parameter);   
}