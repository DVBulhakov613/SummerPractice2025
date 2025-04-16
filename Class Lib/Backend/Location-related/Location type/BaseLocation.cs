using System.ComponentModel.DataAnnotations;

namespace Class_Lib
{
    public abstract class BaseLocation : IHasIdentification
    {
        public uint ID { get; private set; } // could have either -1 or 0 for an undefined location, otherwise positive integers
        public Coordinates GeoData { get; private set; } // location related data (coordinates, address, etc.)
        public List<Employee>? Staff { get; set; } = new(); // staff assigned to this location
        //public List<DeliveryVehicle> Vehicles { get; set; } = new(); // vehicles currently present at this location


        protected internal BaseLocation()
        {
            RowVersion = Array.Empty<byte>();
        }
        public BaseLocation(Coordinates geoData, List<Employee>? staff = null)
        {
            GeoData = geoData;
            if(staff != null)
                Staff = staff;

            RowVersion = Array.Empty<byte>();
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
