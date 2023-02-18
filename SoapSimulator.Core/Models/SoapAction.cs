namespace SoapSimulator.Core.Models;
public class SoapAction
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    [Required]
    public string MethodName { get; set; }
    public RequestFormat RequestFormat { get; set; }
    public ResponseFormat ResponseFormat { get; set; }
    public ActionStatus Status { get; set; }
    public SystemConfiguration SystemConfiguration { get; set; }
    public SoapAction()
    {
        RequestFormat = new();
        ResponseFormat = new();      
        DateCreated = DateTime.Now;
    }
}
