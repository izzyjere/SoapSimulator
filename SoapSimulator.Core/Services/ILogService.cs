namespace SoapSimulator.Core.Services;
public interface ILogService
{
    event EventHandler LogsUpdated;
    Queue<string> Logs { get; }
    void Log(string action, string message);
    void ClearAll();
}
