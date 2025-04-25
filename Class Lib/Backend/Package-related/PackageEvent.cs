using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class PackageEvent
    {
        public uint PackageID { get => Package.ID; private set; }
        public Package Package { get; private set; } // the package to which this event belongs
        public DateTime Timestamp { get; } = DateTime.Now; // logs are always created with the current time
        public BaseLocation Location { get; private set; } // location of the event
        public string Description { get; private set; } // and description of the event

        protected internal PackageEvent()
        {
        }

        public PackageEvent(BaseLocation location, string description, Package package) // only really need to have a constructor, as this class just stores data
        {
            Location = location;
            Description = description;
            Package = package;
        }
    }
}
