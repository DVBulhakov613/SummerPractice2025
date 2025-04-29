using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Employee : Person
    {
        public uint WorkplaceID { get; private set; } // workplace ID (for db purposes)
        public string Position { get; set; }
        public BaseLocation? Workplace { get; set; } // current workplace of the employee
        public User? User { get; set; } // user account of the employee (if any)
        public List<int> Permissions { get; set; } = new();

        protected internal Employee() : base() { }

        public Employee(string name, string surname, string phoneNumber, string position, BaseLocation workplace, string? email = null)
            : base(name, surname, phoneNumber, email)
        {
            Workplace = workplace;
            Position = position;
            WorkplaceID = workplace.ID; // set the workplace ID for db purposes

            AccessService.AssignRolePermissions(this, position); // assign permissions based on the position
        }

        public void AddPermission(AccessService.PermissionKey permissionKey)
        {
            Permissions.Add((int)permissionKey);
        }

        public void RemovePermission(AccessService.PermissionKey permissionKey)
        {
            Permissions.Remove((int)permissionKey);
        }

        public bool HasPermission(AccessService.PermissionKey permissionKey)
        {
            return Permissions.Contains((int)permissionKey);
        }
    }
}
