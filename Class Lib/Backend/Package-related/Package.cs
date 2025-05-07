using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;


namespace Class_Lib
{
    public class Package
    {
        // format of ID:
        // [Postal Code, 5 numbers][Country, 2 letters]-[Day]-[Month]-[Year]-[4 random letters]
        
        public uint ID { get; private set; }
        public PackageStatus PackageStatus { get; set; } = PackageStatus.STORED; // status of the package, set to Created by default
        public DateTime CreatedAt { get; private set; } // the date when the package was created, set at creation time and never changed
        #region Package Properties
        public uint Length { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public double Weight { get; set; } // in kg
        public double Volume { get; private set; } // volume of the package, calculated from length, width and height
        #endregion

        #region Route info // for all this the ui is already done, kind of
        public uint SenderID { get => Sender.ID; private set; } // id of sender for db purposes
        public Client Sender { get; private set; } // sender reference
        public uint ReceiverID { get => Receiver.ID; private set; } // id of receiver for db purposes
        public Client Receiver { get; private set; } // receiver reference
        public uint SentFromID { get => SentFrom.ID; private set; } // id of the department which creates this package for db purposes
        public Warehouse SentFrom { get; private set; } // taken from the department which creates this package
        public uint SentToID { get => SentTo.ID; private set; } // id of the department which receives this package for db purposes
        public Warehouse SentTo { get; set; } // taken from destination
        // public uint CurrentLocationID { get => CurrentLocation.ID; private set; } // id of the department which currently has this package for db purposes
        // public BaseLocation CurrentLocation { get; set; } // default should be SentFrom, but can be changed to any location in the system
        public List<Content> DeclaredContent { get; private set; } = []; // declared contents should not be changed
        public PackageType Type { get; private set; }  // the type of package corresponds to what kind of packaging is used, and is thus immutable
        #endregion

        public List<PackageEvent> Log { get; private set; } = new(); // log of events that happened to this package

        public Package()
        {
            RowVersion = Array.Empty<byte>();
        }

        public Package
            (uint length, uint width, uint height, double weight, Client sender, Client receiver,
            Warehouse sentFrom, Warehouse sentTo, List<Content> declaredContent,
            PackageType type)
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            Sender = sender;
            Receiver = receiver;
            SentFrom = sentFrom;
            SentTo = sentTo;
            // CurrentLocation = currentLocation;
            DeclaredContent = declaredContent;
            Type = type;

            RowVersion = Array.Empty<byte>();
            Log.Add(new PackageEvent(SentFrom, "Посилку створено", this));
            CreatedAt = DateTime.Now; // set the creation date to now
            Volume = Length * Width * Height;
        }

        // all of these may be deprecated due to DB interaction
        //public void GenerateLogEntry(string message, Package package) // adds a log entry to the package
        //{
        //    Log.Add(new PackageEvent(CurrentLocation, message, package));
        //}
        //public void AddContentManual(string name, ContentType type, uint amount)
        //{
        //    DeclaredContent.Add(new Content(name, type, amount, this));
        //}
        //public void AddContent(Content content) // adds content to the package
        //{
        //    if (DeclaredContent.Contains(content))
        //        throw new InvalidOperationException("Ці об'єкти вже існують в посилці.");
        //    else
        //        DeclaredContent.Add(content);
        //}
        //public void RemoveContent(Content content) // removes content from the package
        //{
        //    if (!DeclaredContent.Contains(content))
        //        throw new InvalidOperationException("Об'єкти не існують в посилці.");
        //    else
        //        DeclaredContent.Remove(content);
        //}
        //public void ClearContent() // clears the content of the package
        //{
        //    DeclaredContent.Clear();
        //}


        [Timestamp] // concurrency token property (to avoid synchronization issues)
        public byte[] RowVersion { get; set; }
    }
}
