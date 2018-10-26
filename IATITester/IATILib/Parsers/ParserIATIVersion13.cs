using IATITester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IATITester.IATILib.Parsers
{
    public class ParserIATIVersion13
    {
        public ICollection<IATIActivity> ExtractAcitivities(XDocument xmlDoc)
        {
            List<IATIActivity> activityList = new List<IATIActivity>();
            var activities = from activity in xmlDoc.Descendants("iati-activity")
                             select activity;

            string currency = "";
            foreach(var activity in activities)
            {
                string startDate, endDate = "";
                currency = activity.Attribute("default-currency").Value;
                var dates = activity.Elements("activity-date");
                foreach(var date in dates)
                {
                    if(date.Attribute("type").Value.Equals("start-actual"))
                    {
                        startDate = date.FirstAttribute.Value;
                    }
                    else if(date.Attribute("type").Value.Equals("end-planned"))
                    {
                        endDate = date.FirstAttribute.Value;
                    }
                }

                activityList.Add(new IATIActivity()
                {
                    Identifier = activity.Element("iati-identifier")?.Value,
                    Title = activity.Element("title")?.Value,
                    RecipientCountry = activity.Element("recipient-country")?.Value,
                    RecipientRegion = activity.Element("recipient-region")?.Value,
                    Description = activity.Element("description")?.Value,
                    Sector = activity.Element("sector")?.Value,
                    DefaultCurrency = currency
                });
            }
            return activityList;
        }
    }
}
