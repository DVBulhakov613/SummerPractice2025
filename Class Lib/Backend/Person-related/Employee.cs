using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Employee : Person
    {
        public uint WorkplaceID { get => Workplace.ID; private set; } // workplace ID (for db purposes)
        public string Position { get; set; }
        public  BaseLocation Workplace { get; set; } // current workplace of the employee

        protected internal Employee()
        {
        }

        public Employee(string name, string surname, string phoneNumber, string position, BaseLocation workplace, string? email = null)
            : base(name, surname, phoneNumber, email)
        {
            Position = position;
            Workplace = workplace;
        }
    }
}
