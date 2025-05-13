using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Person_related
{
    public class Client : Person
    {
        public List<Delivery> DeliveriesSent { get; set; } = new List<Delivery>(); // list of packages associated with the client (sender)
        public List<Delivery> DeliveriesReceived { get; set; } = new List<Delivery>(); // list of packages associated with the client (receiver)
        internal Client() // empty constructor for EF core
        { }

        public Client(string firstName, string lastName, string phoneNumber, string email)
            : base(firstName, lastName, phoneNumber, email)
        {
        }


    }
}
