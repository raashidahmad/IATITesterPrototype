using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace IATITester.IATILib.IATIVersion2
{
    public class ParserIATIV2 : IParserIATI
    {
        public static XmlSerializer serializer = new XmlSerializer(typeof(XMLResultVersion2), new XmlRootAttribute("result"));
        /// <summary>
        /// Implements ParseXML
        /// </summary>
        /// <returns></returns>
        public IXMLResult ParseIATIXML(string url)
        {
            IXMLResult xmlResult;

            // Create an XmlNamespaceManager to resolve namespaces.
            NameTable nameTable = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nameTable);
            nsmgr.AddNamespace("iati-extra", "");

            // Create an XmlParserContext.  The XmlParserContext contains all the information
            // required to parse the XML fragment, including the entity information and the
            // XmlNamespaceManager to use for namespace resolution.
            XmlParserContext xmlParserContext = new XmlParserContext(nameTable, nsmgr, null, XmlSpace.None);

            // Create the reader.
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.NameTable = nameTable;

            WebRequest request = WebRequest.Create(url);
            request.Timeout = 20 * 60 * 1000; //Timeout.Infinite;
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = XmlReader.Create(response.GetResponseStream(), xmlReaderSettings, xmlParserContext))
                {
                    xmlResult = (XMLResultVersion2)serializer.Deserialize(reader);
                }
            }

            //using (var Reader = XmlReader.Create(url, xmlReaderSettings, xmlParserContext))
            //{
            //    xmlResult = (XMLResultVersion2)serializer.Deserialize(Reader);
            //}

            return xmlResult;
        }
    }
}
