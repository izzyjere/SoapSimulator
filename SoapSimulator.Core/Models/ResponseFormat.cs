global using System.ComponentModel.DataAnnotations;

namespace SoapSimulator.Core.Models;
public class ResponseFormat
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string BodyFormatXML { get; set; }  
    public DateTime DateCreated { get; set; }
    public SoapAction Action { get; set; }
}
