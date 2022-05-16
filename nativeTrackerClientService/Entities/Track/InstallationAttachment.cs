using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class InstallationAttachment
    {
        public int ID { get; set; }
        public int IMEI { get; set; }
        public byte[] Data { get; set; }

        public virtual Installation IMEINavigation { get; set; }
    }
}
