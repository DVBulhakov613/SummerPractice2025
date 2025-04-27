using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Person : IHasIdentification
    {
        public uint ID { get; private set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get => FirstName + " " + Surname; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        protected internal Person()
        {
            RowVersion = Array.Empty<byte>();
        }

        public Person(string name, string surname, string phoneNumber, string? email = null)
        {
            FirstName = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            if(email != null && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                Email = email;

            RowVersion = Array.Empty<byte>();
        }

        // regex for future use (Email validation) @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        // follows a format of *@*.* where * is any character except for whitespace or @

        [Timestamp] // concurrency token property
        public byte[] RowVersion { get; set; }
    }
}
