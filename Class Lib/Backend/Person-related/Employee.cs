using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Class_Lib
{
    public class Employee : Person
    {
        //public uint? RoleID { get; set; } // role ID (for db purposes)
        //public Role Role { get; set; } // role of the employee (whatever)
        public uint? WorkplaceID { get; private set; } // workplace ID (for db purposes)
        public BaseLocation? Workplace { get; set; } // current workplace of the employee
        public User? User { get; set; } // user account of the employee (if any)
        public List<BaseLocation>? ManagedLocations { get; set; } = new();

        protected internal Employee() : base() { }

        public Employee(string name, string surname, string phoneNumber, string email, BaseLocation? workplace, List<BaseLocation>? managedLocations = null)
            : base(name, surname, phoneNumber, email)
        {
            Workplace = workplace;

            if (workplace != null)
                WorkplaceID = workplace.ID; // set the workplace ID for db purposes
            else
                WorkplaceID = null; // set to null if no workplace is assigned

            if(managedLocations != null)
                ManagedLocations = managedLocations;
        }

        public Employee(string name, string surname, string phoneNumber, string email, string position, BaseLocation? workplace, List<BaseLocation>? managedLocations = null)
            : base(name, surname, phoneNumber, email)
        {
            Workplace = workplace;
            //Position = position;
            if (workplace != null)
                WorkplaceID = workplace.ID; // set the workplace ID for db purposes
            else
                WorkplaceID = null; // set to null if no workplace is assigned

            if (managedLocations != null)
                ManagedLocations = managedLocations;
        }

        //static private string RoleIdToText(uint roleId)
        //{
        //    return roleId switch
        //    {
        //        1 => "Системний Адміністратор",
        //        2 => "Менеджер",
        //        3 => "Працівник",
        //        _ => "Невідома роль"
        //    };
        //}

        //static private uint TextToRoleId(string position)
        //{
        //    return position switch
        //    {
        //        "Системний Адміністратор" => 1,
        //        "Менеджер" => 2,
        //        "Працівник" => 3,
        //        _ => 0
        //    };
        //}

    }
}
