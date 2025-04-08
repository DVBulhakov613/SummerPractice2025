using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject
{
    public class Employee : Person
    {
        public required string Position { get; set; } // position of the employee
        public required BaseLocation Workplace { get; set; } // current workplace of the employee

        public Employee(int id, string name, string surname, string phoneNumber, string? email, string position, BaseLocation workplace)
            : base(id, name, surname, phoneNumber, email)
        {
            Position = position;
            Workplace = workplace;
        }
    }
}
