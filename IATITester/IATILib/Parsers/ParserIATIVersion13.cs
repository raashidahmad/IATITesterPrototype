﻿using IATITester.Models;
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

                //Extracting dates
                var dates = activity.Elements("activity-date");
                foreach(var date in dates)
                {
                    if(date.Attribute("type").Value.Equals("start-actual"))
                    {
                        startDate = date.FirstAttribute?.Value;
                    }
                    else if(date.Attribute("type").Value.Equals("end-planned"))
                    {
                        endDate = date.FirstAttribute?.Value;
                    }
                }

                //Extracting participating organizations
                var organizations = activity.Elements("participating-org");
                List<Organization> organizationList = new List<Organization>();
                foreach(var organization in organizations)
                {
                    organizationList.Add(new Organization()
                    {
                        Name = organization?.Value,
                        Role = organization.Attribute("role")?.Value
                    });
                }

                //Extracting transactions
                var transactions = activity.Elements("transaction");
                List<Transaction> transactionsList = new List<Transaction>();
                foreach(var transaction in transactions)
                {
                    transactionsList.Add(new Transaction()
                    {
                        Amount = transaction.Element("value")?.Value,
                        Currency = transaction.Element("value")?.FirstAttribute.Value,
                        Dated = transaction.Element("transaction-date")?.Attribute("iso-date").Value,
                        AidType = transaction.Element("aid-type")?.Value,
                        TransactionType = transaction.Element("transaction-type")?.Value,
                        Description = transaction.Element("description")?.Value
                    });
                }

                activityList.Add(new IATIActivity()
                {
                    Identifier = activity.Element("iati-identifier")?.Value,
                    Title = activity.Element("title")?.Value,
                    RecipientCountry = activity.Element("recipient-country")?.Value,
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
