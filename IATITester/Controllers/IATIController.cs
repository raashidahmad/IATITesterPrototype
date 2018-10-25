using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using IATITester.IATILib;
using IATITester.IATILib.IATIVersion1;
using IATITester.IATILib.IATIVersion2;
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
            string url = "http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=SOM";
            XmlReader xReader = XmlReader.Create(url, xmlReaderSettings, xmlParserContext);
            XDocument xDoc = XDocument.Load(xReader);
            var activity = (from el in xDoc.Descendants("iati-activity")
                             select el.FirstAttribute).FirstOrDefault();

            string version = "";
            //foreach(var activity in activities)
            //{
                version = activity.Value;
            //}

            /*string jsonText = JsonConvert.SerializeXmlNode(doc);
            dynamic json = JsonConvert.DeserializeObject(jsonText);*/
            /*XElement booksFromFile = XElement.Load("http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=SOM");
            string jsonStr = JsonConvert.SerializeObject(booksFromFile);
            dynamic json = JsonConvert.DeserializeObject(jsonStr);
            /*string activitiesURL;
            IParserIATI parserIATI;
            XMLResultVersion2 returnResult2 = null;
            XMLResultVersion1 returnResult1 = null;

            try
            {
                string country = model.CountryCode;

                if (string.IsNullOrEmpty(model.OrgCode))
                {
                    string org = model.OrgCode;
                    activitiesURL = "http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=" + country + "&reporting-org=" + org + "&stream=True";
                }
                else
                {
                    activitiesURL = "http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=" + country + "&stream=True";
                }

                //Parser v2.01
                //parserIATI = new ParserIATIV2();

                //returnResult2 = (XMLResultVersion2)parserIATI.ParseIATIXML(activitiesURL);

                //var iatiactivityArray = returnResult2?.iatiactivities?.iatiactivity;
                /*if (iatiactivityArray != null && 
                    (iatiactivityArray.n()[0].AnyAttr.n()[0].Value.StartsWith("1.0") ||
                    iatiactivityArray.n()[0].AnyAttr.n()[0].Value.StartsWith("1.03")))
                {
                    //Parser v1.05
                    parserIATI = new ParserIATIV1();
                    returnResult1 = (XMLResultVersion1)parserIATI.ParseIATIXML(activitiesURL);

                    //Conversion
                    ConvertIATIVersion2 convertIATIv2 = new ConvertIATIVersion2();
                    returnResult2 = convertIATIv2.ConvertIATI105to201XML(returnResult1, returnResult2);
                //}
            }
            catch (Exception ex)
            {
                returnResult2.n().Value = ex.Message;
            }
            string jsonStr = JsonConvert.SerializeObject(returnResult2);
            dynamic json = JsonConvert.DeserializeObject(jsonStr);*/
            //string data = JsonConvert.SerializeObject(xReader);
            return Ok(version);
        }
    }
}