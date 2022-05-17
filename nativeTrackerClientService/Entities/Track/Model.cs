using System;
using System.Collections.Generic;

namespace nativeTrackerClientService.Entities
{
    public partial class Model
    {
        public Model()
        {
            InstallationIssues = new HashSet<InstallationIssue>();
            Installations = new HashSet<Installation>();
            Pictures = new HashSet<Picture>();
            Features = new HashSet<Feature>();
        }

        public int ID { get; set; }
        public int ManufacturerID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int InStock { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<InstallationIssue> InstallationIssues { get; set; }
        public virtual ICollection<Installation> Installations { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
    }
}
