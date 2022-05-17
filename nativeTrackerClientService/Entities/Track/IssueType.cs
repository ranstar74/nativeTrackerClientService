using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class IssueType
    {
        public IssueType()
        {
            InstallationIssues = new HashSet<InstallationIssue>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<InstallationIssue> InstallationIssues { get; set; }
    }
}
