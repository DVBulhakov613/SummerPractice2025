using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class LocationDTO
    {
        public string LocationType { get; set; }
        public uint ID { get; set; }
        public CoordinatesDTO Coordinates { get; set; }
        public List<uint>? Staff { get; set; }

        // Warehouse fields
        public uint? MaxStorageCapacity { get; set; }
        public bool? IsAutomated { get; set; }

        // PostalOffice fields
        public bool? HandlesPublicDropOffs { get; set; }
        public bool? IsRegionalHQ { get; set; }
    }
}
