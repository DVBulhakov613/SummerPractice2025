using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject
{
    public abstract class BaseLocation
    {
        public required string LocationID { get; init; } // could have either -1 or 0 for an undefined location, otherwise positive integers
        public required Coordinates GeoData { get; init; } 

        public List<Employee> Staff { get; set; } = new(); // staff assigned to this location
        public List<Package> StoredPackages { get; set; } = new(); // packages stored at this location

        public string LocationType => GetType().Name; // returns the name of the class, which is the type of location
    }
}
