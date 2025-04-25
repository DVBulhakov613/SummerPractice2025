using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Location_related.Interfaces
{
    internal interface IDeliveryHandler
    {
        public List<Package> PackagesSentFromHere { get; set; }
        public List<Package> PackagesSentToHere { get; set; }


        void SendPackage(Package package, PostalOffice sentFrom, PostalOffice sentTo);
        void ReceivePackage(Package package, PostalOffice sentFrom, PostalOffice sentTo);
    }
}
