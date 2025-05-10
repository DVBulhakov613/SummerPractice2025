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
        private Client sender;
        private Package package;
        private Client receiver;
        private Warehouse sentFrom;
        private Warehouse sentTo;

        [Key]
        public uint ID { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now; // date and time of the delivery creation

        public uint PackageID { get; set; }
        public Package Package { get => package; set => package = value ?? throw new ArgumentNullException(nameof(Sender), "Доставка має мати посилку."); }

        public uint SenderID { get; set; }
        public Client Sender { get => sender; set => sender = value ?? throw new ArgumentNullException(nameof(Sender), "Доставка має мати відправника."); }

        public uint ReceiverID { get; set; }
        public Client Receiver { get => receiver; set => receiver = value ?? throw new ArgumentNullException(nameof(Sender), "Доставка має мати отримувача."); }

        public uint SentFromID { get; set; }
        public Warehouse SentFrom { get => sentFrom; set => sentFrom = value ?? throw new ArgumentNullException(nameof(Sender), "Доставка має мати пункт, з якого вона буде відправлена."); }

        public uint SentToID { get; set; }
        public Warehouse SentTo { get => sentTo; set => sentTo = value ?? throw new ArgumentNullException(nameof(Sender), "Доставка має мати пункт, до якого вона буде доставлена."); }

        public uint Price { get; set; } // price of the delivery
        public bool IsPaid { get; set; } = false; // is the delivery paid?

        public List<PackageEvent> Log { get; set; } = [];

        protected Delivery() { } // for EF Core

        public Delivery(Package package, Client sender, Client receiver, Warehouse sentFrom, Warehouse sentTo)
        {
            var exceptions = new List<Exception>();
            try { Sender = sender; }
            catch (Exception ex) { exceptions.Add(ex); }

            try { Receiver = receiver; }
            catch (Exception ex) { exceptions.Add(ex); }

            try { SentFrom = sentFrom; }
            catch (Exception ex) { exceptions.Add(ex); }

            try { SentTo = sentTo; }
            catch (Exception ex) { exceptions.Add(ex); }

            try { Package = package; }
            catch (Exception ex) { exceptions.Add(ex); }

            if (exceptions.Count > 0)
                throw new AggregateException("Помилки при створенні доставки.", exceptions);
            
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
