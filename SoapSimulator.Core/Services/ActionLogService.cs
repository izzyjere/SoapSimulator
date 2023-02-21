namespace SoapSimulator.Core.Services;

public static class ActionLogService 
{
    public static event EventHandler LogsUpdated;
    public static Queue<string> GetLogs() => _logs;
    private static readonly Queue<string> _logs = new();
  
    public static void Log(string action, string message)
    {
        var log = $"{DateTime.Now:dd MMM yyyy H:mm:ss} [{action}] : {message}";
        _logs.Enqueue(log);
        LogsUpdated?.Invoke(null, new EventArgs());
    }

    public static void ClearAll()
    {
       _logs.Clear();
        LogsUpdated?.Invoke(null,new EventArgs());
    }
}