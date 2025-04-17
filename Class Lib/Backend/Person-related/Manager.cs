using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Manager : Employee
    {
        public List<BaseLocation> ManagedLocations { get; set; } = new(); // list of locations managed by this manager
        protected internal Manager() : base() { }
        public Manager(string name, string surname, string phoneNumber, string? email, string position, BaseLocation workplace)
            : base(name, surname, phoneNumber, position, workplace, email)
        {
        }
        //public void AddEmployee(Employee employee, BaseLocation location)
        //{
        //    if (location.Staff.Contains(employee))
        //        throw new InvalidOperationException($"Неможливо додати працівника {employee.ID} до списку локації {location.ID}: працівник вже записаний до списку локації.");
        //    location.AddEmployee(employee);
        //}
        //public void RemoveEmployee(Employee employee, BaseLocation location)
        //{
        //    if (!location.Staff.Contains(employee))
        //        throw new InvalidOperationException("Employee not found in the location's staff.");
        //    location.RemoveEmployee(employee);
        //}
    }
}
