using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class User
    {
        public string Username { get; set; } // username, UNIQUE
        public string PasswordHash { get; set; } // hashed password
        public string Role { get; set; } // "Administrator", "Manager", "Employee", NOT client; wait do we even need this? no prob not
        public uint? PersonID { get; set; } // Foreign key to Person table
        public Employee Employee { get; set; } // Navigation property


        private User() { } // just for EFC

        public User(string username, string passwordHash, string role, Employee employee)
        {
            var exceptions = new List<Exception>();
            if (string.IsNullOrWhiteSpace(username))
                exceptions.Add(new ArgumentNullException(nameof(username), "Username cannot be null or empty."));
            if (string.IsNullOrWhiteSpace(passwordHash))
                exceptions.Add(new ArgumentNullException(nameof(passwordHash), "Password hash cannot be null or empty."));

            if (exceptions.Count > 0)
                throw new AggregateException("Errors while creating user.", exceptions);

            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            Employee = employee;
            PersonID = employee.ID; // set the PersonID to the ID of the employee
        }
    }

}
