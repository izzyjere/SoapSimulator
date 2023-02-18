namespace SoapSimulator.Core.Services;
public interface ILogService
{
    Queue<string> Logs { get; }
    void Log(string action, string message);
    void ClearAll();
}
