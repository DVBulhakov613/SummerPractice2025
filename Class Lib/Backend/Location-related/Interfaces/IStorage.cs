using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public interface IStorage
    {
        List<Package> StoredPackages { get; set; } // packages stored at this location
        uint MaxStorageCapacity { get; set; } // maximum storage capacity
        bool IsFull { get; } // checks if the storage is full

        void StorePackage(Package package); // stores a package in the storage
        void RemovePackage(Package package); // removes a package from the storage
        void ClearStorage(); // clears the storage
    }
}
