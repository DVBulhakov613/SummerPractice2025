using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class User
    {
        public string Username { get; set; } // username, UNIQUE
        public string PasswordHash { get; set; } // hashed password
        public uint RoleID { get; set; } // role ID (for db purposes)
        public Role Role { get; set; } // "Administrator", "Manager", "Employee", NOT client;
        public uint? PersonID { get; set; } // Foreign key to Person table
        public Employee Employee { get; set; } // Navigation property
        [NotMapped] public List<int> CachedPermissions { get; set; } = new();


        private User() { } // just for EFC

        public User(string username, string passwordHash, Role role, Employee employee)
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

        public bool HasPermission(AccessService.PermissionKey permissionKey)
        {
            return CachedPermissions.Contains((int)permissionKey);
        }

        public bool HasPermission(int permissionKey)
        {
            return CachedPermissions.Contains(permissionKey);
        }

        public bool HasPermissions(List<AccessService.PermissionKey> permissionKeys)
        {
            foreach (var permissionKey in permissionKeys)
            {
                if (!HasPermission(permissionKey))
                    return false;
            }
            return true;
        }
    }
}
