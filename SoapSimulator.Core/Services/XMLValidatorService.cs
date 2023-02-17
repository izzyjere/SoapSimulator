using System.Xml;

namespace SoapSimulator.Core.Services;

public class XMLValidatorService : IXMLValidator
{
    public Task<string> Validate(string xml, string xsd)
    {
        try
        {
            XmlReaderSettings Xsettings = new XmlReaderSettings();
            Xsettings.Schemas.Add(null, xsd);
            Xsettings.ValidationType = ValidationType.Schema;
            XmlDocument document = new();
            document.LoadXml(xml);
            XmlReader reader = XmlReader.Create(new StringReader(document.InnerXml), Xsettings);
            while (reader.Read());
            return Task.FromResult(string.Empty);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + e.StackTrace);   
            return Task.FromResult("Invalid XML!!");
        }
    }
}