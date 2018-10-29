using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IATITester.Models
{
    public enum OrganizationRole
    {
         Funding = 1,
         Accountable = 2,
         Extending = 3,
         Implementing = 4
    }

    public class AidTypes
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class TransactionTypes
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CountryOrgModel
    {
        public string CountryCode { get; set; }
        public string OrgCode { get; set; }
    }

    public class Country
    {
        public string Code { get; set; }
        public string ContributionPercentage { get; set; }
    }

    public class Region
    {
        public string Code { get; set; }
        public string ContributionPercentage { get; set; }
    }

    public class Sector
    {
        public string Code { get; set; }
        public string FundPercentage { get; set; }
    }

    public class IATIActivity
    {
        public string Identifier { get; set; }
        public string DefaultCurrency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Sector> Sectors { get; set; }
        public ICollection<Country> Countries { get; set; }
        public ICollection<Region> Regions { get; set; }
        public ICollection<Organization> ParticipatingOrganizations { get; set; }
        public ICollection<IATITransaction> Transactions { get; set; }
    }

    public class Budget
    {
        public string PeriodStart { get; set; }
        public string PeriodEnd { get; set; }
        public string PlannedAmount { get; set; }
    }

    public class Organization
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }

    public class IATITransaction
    {
        public string AidType { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string Dated { get; set; }
        public string Description { get; set; }
    }
}
