using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Content
    {
        private string _name;
        private Package package;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва не може бути пустою.");
                else
                    _name = value.ToUpper();
            }
        } // name given to content (cup, phone, etc)
        public ContentType Type { get; set; }// type of content (electronics, clothing, etc)
        public uint Amount { get; set; }
        public uint PackageID { get; internal set; }
        public Package Package
        {
            get => package;
            internal set => package = value
                ?? throw new ArgumentNullException(nameof(Package), "Зміст посилки має мати посилку, до якої вона належить.");
        } // the package that this content belongs to

        public string Description { get; set; } = string.Empty; // description of the content (if any)
        
        internal Content() => RowVersion = []; // initialize the RowVersion property
        public Content(string name, ContentType type, uint amount, Package package)
        {
            var exceptions = new List<Exception>();

            Name = name.ToUpper();
            Type = type;
            if (amount == 0)
            { exceptions.Add(new ArgumentOutOfRangeException(nameof(Amount), "Існуючий зміст не може мати кількість 0.")); }
            else
            { Amount = amount; }

            Package = package;

            if (exceptions.Count > 0)
                throw new AggregateException("Помилки при створенні посилки.", exceptions);

            RowVersion = new byte[8]; // initialize the RowVersion property
        }

        public Content(string name, string description, ContentType type, uint amount, Package package) : this(name, type, amount, package)
        {
            Description = description;
        }

        [Timestamp] // concurrency token property
        public byte[] RowVersion { get; set; }
    }
}
