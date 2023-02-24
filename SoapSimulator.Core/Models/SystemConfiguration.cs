namespace SoapSimulator.Core.Models;
public class SystemConfiguration
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public bool ValidateRequestBody { get; set; }
    [Required]
    public string Name { get; set; }
    public List<SoapAction> Actions { get; set; }
    public bool IsCurrent { get; set; }
    public SystemConfiguration()
    {
        Actions = new List<SoapAction>();
        Name = $"Config-{DateTime.Now:dd MMM yyyy H:mm}";
        DateCreated = DateTime.Now;
    }
}
