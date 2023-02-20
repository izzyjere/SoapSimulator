namespace SoapSimulator.Core.Models;
public class SoapAction
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    [Required]    
    public string MethodName { get; set; }
    [Required]    
    public string Description { get; set; }
    public RequestFormat Request { get; set; }
    public List<ResponseFormat> Responses { get; set; }
    public ActionStatus Status { get; set; }
    public SystemConfiguration SystemConfiguration { get; set; }
    public SoapAction()
    {
        Request = new();
        Responses = new();      
        DateCreated = DateTime.Now;
    }
    [NotMapped]
    public bool ShowSample { get; set; }
    public ResponseFormat GetResponse(ActionStatus status)    {
       
        return Responses.FirstOrDefault(r => r.ActionStatus == status);
    }

}
