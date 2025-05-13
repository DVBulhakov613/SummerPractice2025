using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class ContentDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public uint Amount { get; set; }
        public uint PackageID { get; set; }
        public string Description { get; set; }
    }
}
