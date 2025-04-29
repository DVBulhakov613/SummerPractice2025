using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Manager : Employee
    {
        public List<BaseLocation> ManagedLocations { get; set; } = new();

        protected internal Manager() : base() { }

        public Manager(string name, string surname, string phoneNumber, string? email, string position, BaseLocation workplace)
            : base(name, surname, phoneNumber, position, workplace, email)
        {
            AccessService.AssignRolePermissions(this, "Менеджер");
        }
    }
}
