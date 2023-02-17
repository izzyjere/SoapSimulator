global using System.ComponentModel.DataAnnotations;

namespace SoapSimulator.Core.Models;
public class ResponseFormat
{
    public Guid Id { get; set; }
    public string XSDPath { get; set; } 
    public string Body { get; set; }  
    public DateTime DateCreated { get; set; }
    public SoapAction Action { get; set; }
}
