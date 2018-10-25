using IATITester.IATILib.IATIVersion1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace IATITester.IATILib
{
    public class ParserIATIV1 : IParserIATI
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ParserIATIV1()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Implements ParseXML
        /// </summary>
        /// <returns></returns>
        public IXMLResult ParseIATIXML(string url)
        {
            IXMLResult xmlResult;
            var serializer = new XmlSerializer(typeof(XMLResultVersion1), new XmlRootAttribute("result"));
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

            using (var Reader = XmlReader.Create(url, xmlReaderSettings, xmlParserContext))
            {
                xmlResult = (XMLResultVersion1)serializer.Deserialize(Reader);
            }

            return xmlResult;
    }
    }
}
