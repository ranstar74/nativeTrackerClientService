using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class ClientUser
    {
        public ClientUser()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
