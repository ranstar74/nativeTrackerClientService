using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Installations = new HashSet<Installation>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string ClientLogin { get; set; }

        public virtual ClientUser ClientLoginNavigation { get; set; }
        public virtual ICollection<Installation> Installations { get; set; }
    }
}
