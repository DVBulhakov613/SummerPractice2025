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
        public uint? RoleID { get; set; } // role ID (for db purposes)
        public Role Role { get; set; } // role of the employee (whatever)
        public uint? WorkplaceID { get; private set; } // workplace ID (for db purposes)
        public BaseLocation? Workplace { get; set; } // current workplace of the employee
        public User? User { get; set; } // user account of the employee (if any)
        public List<int> Permissions { get; set; } = new();
        public List<BaseLocation>? ManagedLocations { get; set; } = new();

        protected internal Employee() : base() { }

        public Employee(string name, string surname, string phoneNumber, string email, string position, BaseLocation? workplace, List<BaseLocation>? managedLocations = null)
            : base(name, surname, phoneNumber, email)
        {
            Workplace = workplace;
            //Position = position;
            if(workplace != null)
                WorkplaceID = workplace.ID; // set the workplace ID for db purposes
            else
                WorkplaceID = null; // set to null if no workplace is assigned

            if(managedLocations != null)
                ManagedLocations = managedLocations;

            AccessService.AssignRolePermissions(this, position); // assign permissions based on the position
        }

        //public void AddPermission(AccessService.PermissionKey permissionKey)
        //{
        //    Permissions.Add((int)permissionKey);
        //}

        //public void RemovePermission(AccessService.PermissionKey permissionKey)
        //{
        //    Permissions.Remove((int)permissionKey);
        //}

        public bool HasPermission(AccessService.PermissionKey permissionKey)
        {
            return Permissions.Contains((int)permissionKey);
        }
    }
}
