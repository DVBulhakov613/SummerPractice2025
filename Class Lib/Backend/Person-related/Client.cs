using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Person_related
{
    public class Client : Person
    {
        public List<Package> PackagesSent { get; set; } = new List<Package>(); // list of packages associated with the client (sender)
        public List<Package> PackagesReceived { get; set; } = new List<Package>(); // list of packages associated with the client (receiver)
        protected internal Client() // empty constructor for EF core
        { }

        public Client(string firstName, string lastName, string phoneNumber, string? email = null)
            : base(firstName, lastName, phoneNumber, email)
        {
        }


    }
}
