using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Person_related
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
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            Employee = employee;
            PersonID = employee.ID; // set the PersonID to the ID of the employee
        }
    }

}
