using System.ComponentModel.DataAnnotations.Schema;

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
    public ResponseFormat Response { get; set; }
    public ActionStatus Status { get; set; }
    public SystemConfiguration SystemConfiguration { get; set; }
    public SoapAction()
    {
        Request = new();
        Response = new();      
        DateCreated = DateTime.Now;
    }
    [NotMapped]
    public bool ShowSample { get; set; }
}
