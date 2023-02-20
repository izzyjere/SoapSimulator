namespace SoapSimulator.Core.Services;

public class XMLValidatorService : IXMLValidator
{
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