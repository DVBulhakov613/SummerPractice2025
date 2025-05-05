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
        private string? _address;
        private string _region;

        public double? Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if(value < -90 || value > 90)
                    throw new ArgumentException("Широта повинна бути між -90 та 90");
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
                    throw new ArgumentException("Довгота повинна бути між -180 та 180");
                _longitude = value;
            }
        }
        public string? Address
        { 
            get
            {
                return _address;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Поле \"Адреса\" не може бути пустою.");
                _address = value;
            }
        }
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Поле \"Регіон\" не може бути пустим.");
                _region = value;
            }
        }
        protected Coordinates()
        {
        }
        public Coordinates(double? latitude, double? longitude, string? address, string region)
        {
            Latitude = latitude;
            Longitude = longitude;
            Address = address;
            Region = region;
        }

        //[Timestamp] // concurrency token property
        //public byte[] RowVersion { get; set; }
    }
}
