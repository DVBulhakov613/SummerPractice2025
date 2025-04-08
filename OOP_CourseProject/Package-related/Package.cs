using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace OOP_CourseProject.Package_related
{
    public class Package
    {
        // format of ID:
        // [Postal Code, 5 numbers][Country, 2 letters]-[Day]-[Month]-[Year]-[4 random letters]
        public required string PackageID { get; init; }
        #region Package Properties
        public int Length { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private int Weight { get; set; }
        #endregion

        #region Route info // for all this the ui is already done, kind of
        public required Person Sender { get; init; } // set at init
        public required Person Receiver { get; init; } // set at init
        public required BaseLocation SentFrom { get; init; } // taken from the department which creates this package
        public required BaseLocation SentTo { get; set; } // taken from destination
        public required Coordinates CurrentLocation { get; set; } // same as SentFrom, but can be changed
        public required List<Content> DeclaredContent { get; init; } = []; // declared contents should not be changed
        public required PackageType Type { get; init; }  // the type of package corresponds to what kind of packaging is used, and is thus immutable
        #endregion

        public required List<PackageEvent> Log { get; init; } = new(); // log of events that happened to this package


        public Package
            (string packageID, int length, int width, int height, int weight, Person sender, Person receiver,
            BaseLocation sentFrom, BaseLocation sentTo, Coordinates currentLocation, List<Content> declaredContent,
            PackageType type)
        {
            PackageID = packageID;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            Sender = sender;
            Receiver = receiver;
            SentFrom = sentFrom;
            SentTo = sentTo;
            CurrentLocation = currentLocation;
            DeclaredContent = declaredContent;
            Type = type;
        }

    }
}
