using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public interface IActionService
{
    ActionResponse ExecuteAction(string actionName);

}
