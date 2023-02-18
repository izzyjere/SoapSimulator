using System.Collections;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SoapSimulator.Core;


public class DynamicXmlObject : DynamicObject, IXmlSerializable
{
    private Dictionary<string, object> _properties;
    public DynamicXmlObject()
    {
          _properties= new Dictionary<string, object>();
    }
    public DynamicXmlObject(Dictionary<string, object> properties)
    {
        _properties = properties;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        _properties[binder.Name] = value;
        return true;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        if (_properties.TryGetValue(binder.Name, out result))
        {
            return true;
        }

        result = null;
        return false;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        _properties.Clear();

        reader.Read();

        while (reader.NodeType != XmlNodeType.EndElement)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                object value;

                if (reader.IsEmptyElement)
                {
                    _properties[reader.LocalName] = null;
                    reader.Read();
                }
                else if (reader.GetAttribute("type") == "null")
                {
                    _properties[reader.LocalName] = null;
                    reader.ReadStartElement();
                }
                else
                {
                    using (var subtree = reader.ReadSubtree())
                    {
                        subtree.Read();
                        value = subtree.ReadString();

                        if (!String.IsNullOrWhiteSpace((string)value))
                        {
                            if (value is string s && int.TryParse(s, out var n))
                            {
                                value = n;
                            }
                            else if (value is string s2 && double.TryParse(s2, out var d))
                            {
                                value = d;
                            }

                            _properties[reader.LocalName] = value;
                        }
                    }

                    reader.ReadEndElement();
                }
            }
            else
            {
                reader.Read();
            }
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var kvp in _properties)
        {
            writer.WriteStartElement(kvp.Key);

            if (kvp.Value == null)
            {
                writer.WriteAttributeString("xsi", "nil", null, "true");
            }
            else if (kvp.Value is DynamicXmlObject)
            {
                // If the property value is another DynamicXmlObject, recursively write it as a child element
                ((DynamicXmlObject)kvp.Value).WriteXml(writer);
            }
            else if (kvp.Value is IList)
            {
                // If the property value is a list, recursively write each item as a child element
                foreach (var item in (IList)kvp.Value)
                {
                    writer.WriteStartElement(kvp.Key);
                    if (item == null)
                    {
                        writer.WriteAttributeString("xsi", "nil", null, "true");
                    }
                    else if (item is DynamicXmlObject)
                    {
                        ((DynamicXmlObject)item).WriteXml(writer);
                    }
                    else
                    {
                        writer.WriteString(item.ToString());
                    }
                    writer.WriteEndElement();
                }
            }
            else
            {
                // Otherwise, write the property value as text content
                writer.WriteString(kvp.Value.ToString());
            }

            writer.WriteEndElement();
        }
    }


    public static dynamic Deserialize(string xml)
    {
        Console.WriteLine($"Deserializing XML:\n{xml}");

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var root = doc.DocumentElement;

        if (root.HasAttributes)
        {
            var dict = new Dictionary<string, object>();
            foreach (XmlAttribute attribute in root.Attributes)
            {
                dict.Add(attribute.Name, attribute.Value);
            }
            Console.WriteLine($"Returning new DynamicXmlObject with attributes:\n{string.Join("\n", dict)}");
            return new DynamicXmlObject(dict);
        }
        else if (root.HasChildNodes)
        {
            var children = root.ChildNodes;
            if (children.Count == 1 && children[0].NodeType == XmlNodeType.Text)
            {
                Console.WriteLine($"Returning value: {children[0].Value}");
                return children[0].Value;
            }
            else
            {
                var dict = new Dictionary<string, dynamic>();
                foreach (XmlNode child in children)
                {
                    if (child.NodeType != XmlNodeType.Element) continue;
                    var key = child.LocalName;
                    if (dict.ContainsKey(key))
                    {
                        if (dict[key] is List<dynamic>)
                        {
                            dict[key].Add(Deserialize(child.OuterXml));
                        }
                        else
                        {
                            var list = new List<dynamic>
                            {
                                dict[key],
                                Deserialize(child.OuterXml)
                            };
                            dict[key] = list;
                        }
                    }
                    else
                    {
                        dict.Add(key, Deserialize(child.OuterXml));
                    }
                }
                Console.WriteLine($"Returning new DynamicXmlObject with properties:\n{string.Join("\n", dict.Select(kv => $"{kv.Key}: {kv.Value}"))}");
                return new DynamicXmlObject(dict);
            }
        }
        else
        {
            Console.WriteLine($"Returning null");
            return null;
        }
    }

}
