using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Package_related
{
    public class Delivery
    {
        [Key]
        public uint ID { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now; // date and time of the delivery creation

        public uint PackageID { get; set; }
        public Package Package { get; set; }

        public uint SenderID { get; set; }
        public Client Sender { get; set; }

        public uint ReceiverID { get; set; }
        public Client Receiver { get; set; }

        public uint SentFromID { get; set; }
        public Warehouse SentFrom { get; set; }

        public uint SentToID { get; set; }
        public Warehouse SentTo { get; set; }

        public uint Price { get; set; } // price of the delivery
        public bool IsPaid { get; set; } = false; // is the delivery paid?

        public List<PackageEvent> Log { get; set; } = [];

        protected Delivery() { } // for EF Core

        public Delivery(Package package, Client sender, Client receiver, Warehouse sentFrom, Warehouse sentTo)
        {
            Sender = sender;
            Receiver = receiver;
            SentFrom = sentFrom;
            SentTo = sentTo;
            Package = package;
            Log.Add(new PackageEvent(SentFrom, "Посилку створено", package)); // package assigned later
            
            RowVersion = [];
        }

        public void AddLog(string message, BaseLocation location, Package package)
        {
            Log.Add(new PackageEvent(location, message, package));
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}
