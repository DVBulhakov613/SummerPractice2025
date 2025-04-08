using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OOP_CourseProject
{
    public class Person
    {
        public int ID { get; init; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; private set; }

        public Person(int id, string name, string surname, string phoneNumber, string? email = null)
        {
            ID = id;
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            if(email != null && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                Email = email;
        }

        // regex for future use (Email validation) @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        // follows a format of *@*.* where * is any character except for whitespace or @

    }
}
