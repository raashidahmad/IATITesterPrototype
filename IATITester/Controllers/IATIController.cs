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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IATITester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IATIController : ControllerBase
    {
        IConfiguration configuration;
        public IATIController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpGet("{countryCode}")]
        public IActionResult GetIATIDataForCountry(string countryCode)
        {
            // Create an XmlNamespaceManager to resolve namespaces.
            NameTable nameTable = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nameTable);
            nsmgr.AddNamespace("iati-extra", "");
            // Create an XmlParserContext.  The XmlParserContext contains all the information
            // required to parse the XML fragment, including the entity information and the
            // XmlNamespaceManager to use for namespace resolution.
            XmlParserContext xmlParserContext = new XmlParserContext(nameTable, nsmgr, null, XmlSpace.None);
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.NameTable = nameTable;
            string url = "http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=" + countryCode;
            XmlReader xReader = XmlReader.Create(url, xmlReaderSettings, xmlParserContext);
            XDocument xDoc = XDocument.Load(xReader);
            var activity = (from el in xDoc.Descendants("iati-activity")
                             select el.FirstAttribute).FirstOrDefault();

            IParser parser;
            ICollection<IATIActivity> activityList = new List<IATIActivity>();
            string version = "";
            version = activity.Value;
            switch(version)
            {
                case "1.03":
                    parser = new ParserIATIVersion13();
                    activityList = parser.ExtractAcitivities(xDoc);
                    break;

                case "2.01":
                    parser = new ParserIATIVersion21(configuration);
                    activityList = parser.ExtractAcitivities(xDoc);
                    break;
            }
            return Ok(activityList);
        }

        
    }
}