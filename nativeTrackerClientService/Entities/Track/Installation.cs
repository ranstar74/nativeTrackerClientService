using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Installation
    {
        public Installation()
        {
            InstallationAttachments = new HashSet<InstallationAttachment>();
            VehicleStates = new HashSet<VehicleState>();
        }

        public int IMEI { get; set; }
        public int VehicleID { get; set; }
        public int ModelID { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual Model Model { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual ICollection<InstallationAttachment> InstallationAttachments { get; set; }
        public virtual ICollection<VehicleState> VehicleStates { get; set; }
    }
}
