namespace SoapSimulator.Core.Services;

public class ActionLogService : ILogService
{
    public event EventHandler LogsUpdated;
    public Queue<string> Logs => _logs;
    private Queue<string> _logs;
    public ActionLogService()
    {
        _logs= new Queue<string>();
        Log("ActionLogService", "Log Service Started.");
    }
    public void Log(string action, string message)
    {
        var log = $"{DateTime.Now:dd MMM yyyy H:mm:ss} [{action}] : {message}";
        _logs.Enqueue(log);
        LogsUpdated?.Invoke(this, new EventArgs());
    }

    public void ClearAll()
    {
       _logs.Clear();
        LogsUpdated?.Invoke(this,new EventArgs());
    }
}