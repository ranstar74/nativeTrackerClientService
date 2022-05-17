using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class IssuePayment
    {
        public IssuePayment()
        {
            InstallationIssues = new HashSet<InstallationIssue>();
        }

        public int ID { get; set; }
        public decimal Cost { get; set; }
        public bool IsPayed { get; set; }
        public DateTime? PayTimestamp { get; set; }

        public virtual ICollection<InstallationIssue> InstallationIssues { get; set; }
    }
}
