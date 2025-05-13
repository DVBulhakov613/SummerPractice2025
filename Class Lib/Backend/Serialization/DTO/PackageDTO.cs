using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class PackageDTO
    {
        public uint ID { get; set; }
        public string PackageStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Length { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public double Weight { get; set; }
        public List<ContentDTO> DeclaredContent { get; set; } = [];
        public string Type { get; set; }
        public uint DeliveryID { get; set; }
    }
}
