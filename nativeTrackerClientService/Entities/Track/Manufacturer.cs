﻿using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Models = new HashSet<Model>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; set; }
    }
}
