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
        }

        //public void AddLocation(BaseLocation location, Manager role)
        //{
        //    if(!role.ManagedLocations.Contains(location))
        //        ManagedLocations.Add(location);
        //    else throw new InvalidOperationException("Неможливо додати локацію до списку: вже існує в списку.");
        //}
        //public void RemoveLocation(BaseLocation location, Manager role)
        //{
        //    if (role.ManagedLocations.Contains(location))
        //        ManagedLocations.Remove(location);
        //    else throw new InvalidOperationException("Неможливо видалити локацію зі списку: не існує в списку.");
        //}
    }
}
