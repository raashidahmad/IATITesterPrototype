using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IATITester.IATILib
{
    public partial class Log
    {
        public int Id { get; set; }
        public Nullable<int> LogType { get; set; }
        public string OrgId { get; set; }
        public string IatiIdentifier { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Message { get; set; }
        public string ExceptionObj { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
