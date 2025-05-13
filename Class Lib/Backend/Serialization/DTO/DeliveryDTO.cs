using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class DeliveryDTO
    {
        public uint ID { get; set; }
        public DateTime Timestamp { get; set; }
        public PackageDTO Package { get; set; }
        public uint SenderID { get; set; }
        public uint ReceiverID { get; set; }
        public uint SentFromID { get; set; }
        public uint SentToID { get; set; }
        public uint Price { get; set; }
        public bool IsPaid { get; set; }
    }
}
