namespace SoapSimulator.Core.Models;
public class RequestFormat
{
    public Guid Id { get; set; }
    public string Body { get; set; } 
    public string XMLFileName { get; set; }
    public DateTime DateCreated { get; set; }
    public SoapAction Action { get; set; }
    public RequestFormat()
    {
        DateCreated = DateTime.Now;
    }

}
