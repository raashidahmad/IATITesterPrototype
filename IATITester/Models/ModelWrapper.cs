using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IATITester.Models
{
    public class CountryOrgModel
    {
        public string CountryCode { get; set; }
        public string OrgCode { get; set; }
    }

    public class IATIActivity
    {
        public string Identifier { get; set; }
        public string DefaultCurrency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientRegion { get; set; }
        public string Sector { get; set; }
        public ICollection<Organization> ParticipatingOrganizations { get; set; }
    }

    public class Organization
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
