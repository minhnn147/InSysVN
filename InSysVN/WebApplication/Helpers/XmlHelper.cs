using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Service
{
    public class XmlHelper
    {
        public static T DeserializeXML<T>(string Xml)
        {

            XmlSerializer ser;
            ser = new XmlSerializer(typeof(T));
            StringReader stringReader;
            stringReader = new StringReader(Xml);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            T obj;
            obj = (T)ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            return obj;
        }

        //Serializes the <i>Obj</i> to an XML string.
        public static string SerializeXML<T>(object Obj)
        {

            XmlSerializer ser;
            ser = new XmlSerializer(typeof(T));
            MemoryStream memStream;
            memStream = new MemoryStream();
            XmlTextWriter xmlWriter;
            xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            xmlWriter.Namespaces = true;
            ser.Serialize(xmlWriter, Obj);
            xmlWriter.Close();
            memStream.Close();
            string xml;
            xml = Encoding.UTF8.GetString(memStream.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "")
                .Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                .Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
        }

        public static string SerializeXMLWithNoSpace<T>(object Obj)
        {

            XmlSerializer ser;
            // instantiate the container for all attribute overrides
            XmlAttributeOverrides xOver = new XmlAttributeOverrides();

            // define a set of XML attributes to apply to the root element
            XmlAttributes xAttrs1 = new XmlAttributes();

            // define an XmlRoot element (as if [XmlRoot] had decorated the type)
            // The namespace in the attribute override is the empty string. 
            XmlRootAttribute xRoot = new XmlRootAttribute() { Namespace = "" };

            // add that XmlRoot element to the container of attributes
            xAttrs1.XmlRoot = xRoot;

            // add that bunch of attributes to the container holding all overrides
            xOver.Add(typeof(T), xAttrs1);
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");

            ser = new XmlSerializer(typeof(T), xOver);
            MemoryStream memStream;
            memStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.Encoding = UTF8Encoding.UTF8;
            XmlWriter xmlWriter = XmlWriter.Create(memStream, settings);
            //XmlTextWriter xmlWriter;
            //xmlWriter = new XmlTextWriter(memStream, UTF8Encoding.UTF8);
            //xmlWriter.Namespaces = true;
            ser.Serialize(xmlWriter, Obj, ns);
            xmlWriter.Close();
            memStream.Close();
            string xml;
            xml = Encoding.UTF8.GetString(memStream.ToArray());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            //return xml;
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "")
                .Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                .Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
        }

        public static string CleanEmptyTags(String xml)
        {
            Regex regex = new Regex(@"(\s)*<(\w)*(\s)*/>");
            return regex.Replace(xml, string.Empty);
        }

        /// <summary>
        /// Serializes the object to XML based on encoding and name spaces.
        /// </summary>
        /// <param name="serializer">XmlSerializer object 
        /// (passing as param to avoid creating one every time)</param>
        /// <param name="encoding">The encoding of the serialized Xml</param>
        /// <param name="ns">The namespaces to be used by the serializer</param>
        /// <param name="omitDeclaration">Whether to omit Xml declarartion or not</param>
        /// <param name="objectToSerialize">The object we want to serialize to Xml</param>
        /// <returns></returns>
        public static string Serialize(XmlSerializer serializer,
                                       Encoding encoding,
                                       XmlSerializerNamespaces ns,
                                       bool omitDeclaration,
                                       object objectToSerialize)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = omitDeclaration;
            settings.Encoding = encoding;
            XmlWriter writer = XmlWriter.Create(ms, settings);
            serializer.Serialize(writer, objectToSerialize, ns);
            return encoding.GetString(ms.ToArray()); ;
        }
    }
}
