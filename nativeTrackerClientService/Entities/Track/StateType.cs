using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class StateType
    {
        public StateType()
        {
            VehicleStates = new HashSet<VehicleState>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<VehicleState> VehicleStates { get; set; }
    }
}
