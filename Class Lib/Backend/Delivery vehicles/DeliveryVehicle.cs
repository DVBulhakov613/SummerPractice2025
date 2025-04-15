using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Delivery_vehicles
{
    public class DeliveryVehicle : IHasIdentification, IStorage
    {
        public uint ID { get; private set; } // unique identifier for the object
        public uint MaxStorageCapacity { get; set; } // maximum storage capacity of the vehicle (in this case, weight)
        public uint MaxVolume { get; set; } // maximum volume of the vehicle (in this case, volume)
        public List<Package> Packages { get; set; } = []; // list of packages in the vehicle

        public List<Package> StoredPackages { get; set; } = [];

        public bool IsFull => Packages.Sum(p => p.Weight) == MaxStorageCapacity || Packages.Sum(p => p.Volume) == MaxVolume;

        protected DeliveryVehicle()
        {
        }

        public DeliveryVehicle(uint id, uint maxStorageCapacity)
        {
            ID = id;
            MaxStorageCapacity = maxStorageCapacity;
        }
        public void StorePackage(Package package)
        {
            if ((Packages.Sum(p => p.Weight) + package.Weight ) >= MaxStorageCapacity || (Packages.Sum(p => p.Volume) + package.Volume) >= MaxVolume)
                throw new InvalidOperationException("Vehicle storage is full.");
            Packages.Add(package);
        }
        public void RemovePackage(Package package)
        {
            if (!Packages.Contains(package))
                throw new InvalidOperationException("Package not found in vehicle storage.");
            Packages.Remove(package);
        }
        public void ClearStorage()
        {
            Packages.Clear();
        }
    }
}
