using Class_Lib.Backend.Delivery_vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public abstract class BaseLocation : IHasIdentification
    {
        public uint ID { get; private set; } // could have either -1 or 0 for an undefined location, otherwise positive integers
        public Coordinates GeoData { get; private set; } // location related data (coordinates, address, etc.)
        public List<Employee> Staff { get; set; } = new(); // staff assigned to this location
        public List<DeliveryVehicle> Vehicles { get; set; } = new(); // vehicles currently present at this location


        protected BaseLocation()
        {
        }
        public BaseLocation(uint id, Coordinates geoData)
        {
            ID = id;
            GeoData = geoData;
        }
        public BaseLocation(uint id, Coordinates geoData, List<Employee> staff)
        {
            ID = id;
            GeoData = geoData;
            Staff = staff;
        }

        public void AddEmployee(Employee employee)
        {
            if (!Staff.Contains(employee))
            {
                Staff.Add(employee);
            }
            else
            {
                throw new ArgumentException("Працівник вже існує в списку.");
            }
        }
        public void RemoveEmployee(Employee employee)
        {
            if (Staff.Contains(employee))
            {
                Staff.Remove(employee);
            }
            else
            {
                throw new ArgumentException("Вказаного працівника не знайдено в списку.");
            }
        }
        public void UpdateGeoData(Coordinates newGeoData)
        {
            GeoData = newGeoData;
        }

        [Timestamp] // concurrency token property
        public byte[] RowVersion { get; set; }
        public string LocationType => GetType().Name; // returns the name of the class, which is the type of location
    }
}
