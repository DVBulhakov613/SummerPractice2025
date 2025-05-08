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
        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.STORED;

        public uint SenderID => Sender.ID;
        public Client Sender { get; private set; }

        public uint ReceiverID => Receiver.ID;
        public Client Receiver { get; private set; }

        public uint SentFromID => SentFrom.ID;
        public Warehouse SentFrom { get; private set; }

        public uint SentToID => SentTo.ID;
        public Warehouse SentTo { get; set; }

        public List<PackageEvent> Log { get; private set; } = [];

        public Delivery(Client sender, Client receiver, Warehouse sentFrom, Warehouse sentTo)
        {
            Sender = sender;
            Receiver = receiver;
            SentFrom = sentFrom;
            SentTo = sentTo;
            Log.Add(new PackageEvent(SentFrom, "Посилку створено", null)); // you'll assign Package later
        }

        public void AddLog(string message, BaseLocation location, Package package)
        {
            Log.Add(new PackageEvent(location, message, package));
        }
    }

}
