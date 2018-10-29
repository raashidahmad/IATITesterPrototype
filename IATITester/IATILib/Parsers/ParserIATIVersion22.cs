using IATITester.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IATITester.IATILib.Parsers
{
    public class ParserIATIVersion22
    {
        IConfiguration configuration;
        ICollection<AidTypes> aidTypes;
        ICollection<TransactionTypes> transactionTypes;
        public ParserIATIVersion22(IConfiguration config)
        {
            this.configuration = config;
            aidTypes = JsonConvert.DeserializeObject<List<AidTypes>>(configuration["aidTypes"]);
            transactionTypes = JsonConvert.DeserializeObject<List<TransactionTypes>>(configuration["transactionTypes"]);
        }

        public ICollection<IATIActivity> ExtractAcitivities(XDocument xmlDoc)
        {
            List<IATIActivity> activityList = new List<IATIActivity>();
            var activities = from activity in xmlDoc.Descendants("iati-activity")
                             select activity;

            string currency = "";
            foreach (var activity in activities)
            {
                string startDate, endDate = "";
                currency = activity.Attribute("default-currency").Value;

                //Extracting dates
                var dates = activity.Elements("activity-date");
                foreach (var date in dates)
                {
                    if (date.Attribute("type").Value.Equals("start-actual"))
                    {
                        startDate = date.FirstAttribute?.Value;
                    }
                    else if (date.Attribute("type").Value.Equals("end-planned"))
                    {
                        endDate = date.FirstAttribute?.Value;
                    }
                }

                //Extracting participating organizations
                var organizations = activity.Elements("participating-org");
                List<Organization> organizationList = new List<Organization>();
                foreach (var organization in organizations)
                {
                    var role = (OrganizationRole)Enum.Parse(typeof(OrganizationRole), organization.Attribute("role").Value);
                    var narratives = organization.Elements("narrative");
                    var organizationName = (from n in narratives
                                           where n.FirstAttribute.Value == "en"
                                           select n.FirstAttribute.Value).FirstOrDefault();

                    organizationList.Add(new Organization()
                    {
                        Name = organizationName,
                        Role = role.ToString()
                    });
                }

                //Extracting transactions
                var transactions = activity.Elements("transaction");
                List<IATITransaction> transactionsList = new List<IATITransaction>();
                foreach (var transaction in transactions)
                {
                    string aidTypeCode = transaction.Element("aid-type")?.Value;
                    string transactionCode = transaction.Element("transaction-type")?.Value;

                    string aidType = (from t in aidTypes
                                      where t.Code.Equals(aidTypeCode)
                                      select t.Name).FirstOrDefault();
                    string transactionType = (from t in transactionTypes
                                              where t.Code.Equals(transactionCode)
                                              select t.Name).FirstOrDefault();

                    transactionsList.Add(new IATITransaction()
                    {
                        Amount = transaction.Element("value")?.Value,
                        Currency = transaction.Element("value")?.FirstAttribute.Value,
                        Dated = transaction.Element("transaction-date")?.Attribute("iso-date").Value,
                        AidType = aidType,
                        TransactionType = transactionType,
                        Description = transaction.Element("description")?.Value
                    });
                }

                var recipientCountries = activity.Elements("recipient-country");
                List<Country> countries = new List<Country>();
                foreach(var country in recipientCountries)
                {
                    countries.Add(new Country()
                    {
                        Code = country.Attribute("code").Value,
                        ContributionPercentage = country.Attribute("percentage").Value
                    });
                }

                activityList.Add(new IATIActivity()
                {
                    Identifier = activity.Element("iati-identifier")?.Value,
                    Title = activity.Element("title")?.Value,
                    Countries = countries,
                    RecipientRegion = activity.Element("recipient-region")?.Value,
                    Description = activity.Element("description")?.Value,
                    Sector = activity.Element("sector")?.Value,
                    DefaultCurrency = currency,
                    Transactions = transactionsList,
                    ParticipatingOrganizations = organizationList
                });
            }
            return activityList;
        }
    }
}
