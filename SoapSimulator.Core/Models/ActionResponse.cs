using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SoapSimulator.Core.Models;

[DataContract]
public class ActionResponse : IActionResponse
{
    [DataMember]
    public string Body { get; set; }
}
public interface IActionResponse
{
    string Body { get; set; }
}
