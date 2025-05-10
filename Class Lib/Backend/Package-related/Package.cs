using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;


namespace Class_Lib.Backend.Package_related
{
    public class Package
    {
        private uint length;
        private uint width;
        private uint height;
        private double weight;

        public uint ID { get; private set; }
        public PackageStatus PackageStatus { get; set; } = PackageStatus.STORED;
        public DateTime CreatedAt { get; private set; }

        public uint Length
        {
            get => length;
            set
            {
                if (value == 0)
                    throw new ArgumentOutOfRangeException(nameof(Length), "Довжина повинна бути більше 0.");
                length = value;
            }
        }

        public uint Width
        {
            get => width;
            set
            {
                if (value == 0)
                    throw new ArgumentOutOfRangeException(nameof(Width), "Ширина повинна бути більше 0.");
                width = value;
            }
        }

        public uint Height
        {
            get => height;
            set
            {
                if (value == 0)
                    throw new ArgumentOutOfRangeException(nameof(Height), "Висота повинна бути більше 0.");
                height = value;
            }
        }

        [NotMapped]
        public string Dimensions => $"{Length}x{Width}x{Height}";

        public double Weight
        {
            get => weight;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Weight), "Вага повинна бути більше 0.");
                weight = value;
            }
        }

        [NotMapped]
        public double Volume => Length * Width * Height;

        public List<Content> DeclaredContent { get; private set; } = [];

        public PackageType Type { get; private set; }

        public Delivery Delivery { get; set; }


        protected Package() => RowVersion = Array.Empty<byte>();

        public Package(uint length, uint width, uint height, double weight, 
            Client sender, Client receiver, Warehouse sentFrom, Warehouse sentTo, 
            List<Content> declaredContent, PackageType type)
        {
            var exceptions = new List<Exception>();

            try
            { Length = length; }
            catch (Exception ex)
            { exceptions.Add(ex); }
            
            try
            { Width = width; }
            catch (Exception ex)
            { exceptions.Add(ex); }

            try
            { Height = height; }
            catch (Exception ex)
            { exceptions.Add(ex); }

            try
            { Weight = weight; }
            catch (Exception ex)
            { exceptions.Add(ex); }

            if (exceptions.Count > 0)
                throw new AggregateException("Помилки при створенні посилки.", exceptions);

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
