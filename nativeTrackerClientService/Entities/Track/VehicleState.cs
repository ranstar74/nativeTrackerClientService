using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class VehicleState
    {
        public int ID { get; set; }
        public int IMEI { get; set; }
        public DateTime Timestamp { get; set; }
        public int StateTypeID { get; set; }
        public byte[] Data { get; set; }

        public virtual Installation IMEINavigation { get; set; }
        public virtual StateType StateType { get; set; }
    }
}
