namespace SoapSimulator.Core.Services;
public interface IXMLValidator
{
    XMLValidationResponse IsValidXml(string xmlString);
}
