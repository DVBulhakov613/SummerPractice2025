using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class RoleDTO
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public List<uint> Permissions { get; set; }
    }
}
