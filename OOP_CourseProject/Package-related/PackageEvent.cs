using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Package_related
{
    public class PackageEvent
    {
        public required DateTime Timestamp { get; init; }
        public required BaseLocation Location { get; init; }
        public required string Description { get; init; }

        public PackageEvent(DateTime timestamp, BaseLocation location, string description)
        {
            Timestamp = DateTime.Now;
            Location = location;
            Description = description;
        }
    }
}
