using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapSimulator.Core.Services;
public interface ILogService
{
    Queue<string> Logs { get; }
    void Log(string action, string message);
    void ClearAll();
}
