using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            InstallationIssues = new HashSet<InstallationIssue>();
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patrynomic { get; set; }
        public byte[] Photo { get; set; }
        public int? RoleID { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<InstallationIssue> InstallationIssues { get; set; }
    }
}
