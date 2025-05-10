using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class PackageEvent
    {
        private Package package;
        private BaseLocation location;

        public uint PackageID { get; private set; }
        public Package Package 
        { 
            get => package; 
            private set => package = value 
                ?? throw new ArgumentNullException(nameof(Package), "Подія має мати посилку, до якої вона належить."); 
        } // the package to which this event belongs
        public DateTime Timestamp { get; } = DateTime.Now; // logs are always created with the current time
        
        public uint LocationID { get; private set; } // location of the event
        public BaseLocation Location
        {
            get => location;
            private set => location = value
                ?? throw new ArgumentNullException(nameof(Package), "Подія має мати локацію, в якій вона сталась.");
        } // location of the event

        public string Description { get; private set; } // and description of the event

        protected internal PackageEvent()
        {
        }

        public PackageEvent(BaseLocation location, string description, Package package) // only really need to have a constructor, as this class just stores data
        {
            var exceptions = new List<Exception>();

            try { Package = package; }
            catch (Exception ex) { exceptions.Add(ex); }

            try { Location = location; }
            catch (Exception ex) { exceptions.Add(ex); }

            if (exceptions.Count > 0)
                throw new AggregateException("Помилки при створенні події.", exceptions);

            Description = description;
            
        }
    }
}
