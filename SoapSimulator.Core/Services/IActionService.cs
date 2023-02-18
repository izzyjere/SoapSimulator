using SoapSimulator.Core.Models;

namespace SoapSimulator.Core.Services;
public interface IActionService
{
    IActionResponse ExecuteAction(string actionName);

}
