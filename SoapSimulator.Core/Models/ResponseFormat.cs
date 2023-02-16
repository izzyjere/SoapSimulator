global using System.ComponentModel.DataAnnotations;

namespace SoapSimulator.Core.Models;
public class ResponseFormat
{
    [Key]
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string BodyFormatXML { get; set; }
    public bool IsInUse { get; set; }
    public DateTime DateCreated { get; set; }
}
