using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Administrator : Manager
    {
        protected internal Administrator() : base() { }

        public Administrator(string name, string surname, string phoneNumber, string? email, string position, BaseLocation workplace)
            : base(name, surname, phoneNumber, email, position, workplace)
        {
            // Assign default permissions for Administrator
            AccessService.AssignRolePermissions(this, "Системний Адміністратор");
        }
    }

}
