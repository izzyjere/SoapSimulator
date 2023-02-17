namespace SoapSimulator.Core.Models;
public class RequestFormat
{
    public Guid Id { get; set; }
    public string BodyFormatXML { get; set; }
    public bool IsInUse { get; set; }
    public DateTime DateCreated { get; set; }
    public SoapAction Action { get; set; }

}
