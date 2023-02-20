using System.Xml;

namespace SoapSimulator.Core.Services;
public interface IXMLValidator
{
    Task<string> Validate(string xml,string xsdpath);
    public XMLValidationResponse IsValidXml(string xmlString)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            return new XMLValidationResponse(true);
        }
        catch (XmlException e)
        {
            return new XMLValidationResponse(false, e.Message);
        }
    }
}
public record XMLValidationResponse(bool IsValid, string Message="");