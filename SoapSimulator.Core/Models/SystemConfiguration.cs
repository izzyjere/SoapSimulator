namespace SoapSimulator.Core.Models;
public class SystemConfiguration
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public string Name { get; set; }
    public List<SoapAction> Actions { get; set; }
    public bool IsCurrent { get; set; }
}
