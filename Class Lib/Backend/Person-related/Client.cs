using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Person_related
{
    public class Client : Person
    {
        public List<Package> Packages { get; set; } = new List<Package>(); // list of packages associated with the client (sender / receiver)
        protected Client() // empty constructor for EF core
        { }

        public Client(uint id, string firstName, string lastName, string email, string phoneNumber)
            : base(id, firstName, lastName, email, phoneNumber)
        {
        }


    }
}
