using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class InstallationIssue
    {
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public int IssueTypeID { get; set; }
        public int ClientID { get; set; }
        public int? ManagerID { get; set; }
        public int StatusID { get; set; }
        public int? WorkerID { get; set; }
        public int? ModelID { get; set; }
        public int? IMEI { get; set; }
        public int? IssuePaymentID { get; set; }
        public string Comments { get; set; }
        public string CloseReason { get; set; }

        public virtual IssuePayment IssuePayment { get; set; }
        public virtual IssueType IssueType { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual Model Model { get; set; }
        public virtual IssueStatus Status { get; set; }
        public virtual Role Worker { get; set; }
    }
}
