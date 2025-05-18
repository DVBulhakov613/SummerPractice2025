using System.ComponentModel.DataAnnotations;

namespace Class_Lib
{
    public abstract class BaseLocation : IHasIdentification
    {
        public uint ID { get; internal set; } // could have either -1 or 0 for an undefined location, otherwise positive integers
        public Coordinates GeoData { get; internal set; } // location related data (coordinates, address, etc.)
        public List<Employee>? Staff { get; set; } = []; // staff assigned to this location
        //public List<DeliveryVehicle> Vehicles { get; set; } = new(); // vehicles currently present at this location


        internal BaseLocation()
        {
            RowVersion = Array.Empty<byte>();
        }
        public BaseLocation(Coordinates geoData, List<Employee>? staff = null)
        {
            var exceptions = new List<Exception>();

            if (geoData == null)
                exceptions.Add(new ArgumentNullException(nameof(geoData), "Локація повинна мати геологічні дані."));
            else
                GeoData = geoData;
            if (exceptions.Count > 0)
                throw new AggregateException("Помилки при створенні локації.", exceptions);

            if (staff != null)
                Staff = staff;

            RowVersion = Array.Empty<byte>();
        }

        public void AddEmployee(Employee employee)
        {
            if (Staff != null ? Staff.Contains(employee) : throw new ArgumentException("Локація не має працівників."))
            {
                throw new ArgumentException("Працівник вже існує в списку.");
            }
            else
            {
                Staff.Add(employee);
            }
        }
        public void RemoveEmployee(Employee employee)
        {
            if (Staff != null ? !Staff.Remove(employee) : throw new ArgumentException("Локація не має працівників."))
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
