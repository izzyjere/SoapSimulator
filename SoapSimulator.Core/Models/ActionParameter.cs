namespace SoapSimulator.Core.Models;

public class ActionParameter
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public SoapAction Action { get; set; }
}
