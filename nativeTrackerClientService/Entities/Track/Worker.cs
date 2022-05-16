using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Worker
    {
        public Worker()
        {
            Installations = new HashSet<Installation>();
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patrynomic { get; set; }
        public byte[] Photo { get; set; }

        public virtual ICollection<Installation> Installations { get; set; }
    }
}
