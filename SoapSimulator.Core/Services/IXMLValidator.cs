namespace SoapSimulator.Core.Services;
public interface IXMLValidator
{
    Task<string> Validate(string xml,string xsdpath);
}
