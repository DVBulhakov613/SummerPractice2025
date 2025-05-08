using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;


namespace Class_Lib.Backend.Package_related
{
    public class Package
    {
        public uint ID { get; private set; }
        public PackageStatus PackageStatus { get; set; } = PackageStatus.STORED;
        public DateTime CreatedAt { get; private set; }

        public uint Length { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        [NotMapped] public string Dimensions => $"{Length}x{Width}x{Height}"; // string representation of dimensions
        public double Weight { get; set; }
        [NotMapped] public double Volume { get => Length * Width * Height; }

        public List<Content> DeclaredContent { get; private set; } = [];
        public PackageType Type { get; private set; }

        public uint DeliveryID { get; set; } // foreign key for Delivery
        public Delivery Delivery { get; set; }

        protected Package() => RowVersion = Array.Empty<byte>();

        public Package(uint length, uint width, uint height, double weight, 
            Client sender, Client receiver, Warehouse sentFrom, Warehouse sentTo, 
            List<Content> declaredContent, PackageType type)
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            DeclaredContent = declaredContent;
            Type = type;

            CreatedAt = DateTime.Now;
            RowVersion = Array.Empty<byte>();

            Delivery = new Delivery(this, sender, receiver, sentFrom, sentTo);
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}
