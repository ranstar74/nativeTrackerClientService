using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Picture
    {
        public int ID { get; set; }
        public int ModelID { get; set; }
        public byte[] Data { get; set; }

        public virtual Model Model { get; set; }
    }
}
