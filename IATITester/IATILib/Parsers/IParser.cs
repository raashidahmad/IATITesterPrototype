using IATITester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IATITester.IATILib.Parsers
{
    public interface IParser
    {
        ICollection<IATIActivity> ExtractAcitivities(XDocument xmlDoc);
    }
}
