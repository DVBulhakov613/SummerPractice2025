using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class EmployeeDTO
    {
        public uint ID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public uint? WorkplaceID { get; set; }
        public List<uint> ManagedLocations { get; set; }
    }
}
