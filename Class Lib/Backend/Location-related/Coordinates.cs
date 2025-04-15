using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Coordinates
    {
        private double? _latitude;
        private double? _longitude;
        public double? Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if(value < -90 || value > 90)
                    throw new ArgumentOutOfRangeException("Latitude must be between -90 and 90");
                _latitude = value;
            }
        }
        public double? Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if(value < -180 || value > 180)
                    throw new ArgumentOutOfRangeException("Longitude must be between -180 and 180");
            }
        }
        public string? Address
        { get; set; }
        public string Region
        { get; set; }
        public string Country // should be pulled from db? do we need this?
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Coordinates(double? latitude, double? longitude, string? address, string region, string country)
        {
            Latitude = latitude;
            Longitude = longitude;
            Address = address;
            Region = region;
            Country = country;
        }

        [Timestamp] // concurrency token property
        public byte[] RowVersion { get; set; }
    }
}
