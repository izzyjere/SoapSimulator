using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SoapSimulator.Core.Models;
public class ActionRequest : IActionRequest
{
    [DataMember]
    public string Body { get; set; }
}
public interface IActionRequest
{
    string Body { get; set; }
}
