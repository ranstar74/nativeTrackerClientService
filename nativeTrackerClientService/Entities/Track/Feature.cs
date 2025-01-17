﻿using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Feature
    {
        public Feature()
        {
            VehicleStates = new HashSet<VehicleState>();
            Models = new HashSet<Model>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<VehicleState> VehicleStates { get; set; }
        public virtual ICollection<Model> Models { get; set; }
    }
}
