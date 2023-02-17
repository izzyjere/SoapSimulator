namespace SoapSimulator.Core.Models;
public class SoapAction
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public string MethodName { get; set; }
    public RequestFormat RequestFormat { get; set; }
    public ResponseFormat ResponseFormat { get; set; }
    public ActionStatus Status { get; set; }
    public List<ActionParameter> Parameters { get; set; }
    public SystemConfiguration SystemConfiguration { get; set; }
    public SoapAction()
    {
        RequestFormat = new();
        ResponseFormat = new();
        Parameters= new List<ActionParameter>();
    }
}
