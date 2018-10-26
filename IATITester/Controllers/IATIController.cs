using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using IATITester.IATILib;
using IATITester.IATILib.Parsers;
using IATITester.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IATITester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IATIController : ControllerBase
    {
        public IATIController()
        {

        }

        [HttpPost]
        public IActionResult SearchByCountryOrg([FromBody] CountryOrgModel model)
        {
            //var serializer = new XmlSerializer(typeof(XMLResultVersion1), new XmlRootAttribute("result"));
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
            string url = "http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=SOM";
            XmlReader xReader = XmlReader.Create(url, xmlReaderSettings, xmlParserContext);
            XDocument xDoc = XDocument.Load(xReader);
            var activity = (from el in xDoc.Descendants("iati-activity")
                             select el.FirstAttribute).FirstOrDefault();

            ParserIATIVersion13 parser = new ParserIATIVersion13();
            var activityList = parser.ExtractAcitivities(xDoc);

            string version = "";
            version = activity.Value;

            return Ok(activityList);
        }

        
    }
}