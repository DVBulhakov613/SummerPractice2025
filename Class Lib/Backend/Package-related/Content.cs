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
        public string Name
        {
            get => _name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty");
                else
                    _name = value.ToUpper();
            }
        } // name given to content (cup, phone, etc)
        private ContentType _type;
        public ContentType Type // type of content (electronics, clothing, etc)
        {
            get => _type;
            private set => _type = value;
        }
        private uint _amount;
        public uint Amount // number of items
        {
            get => _amount;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Amount cannot be negative");
                else
                    _amount = value;
            }
        }
        public uint PackageID { get => Package.ID; }
        public Package Package { get; set; } // the package that this content belongs to
        private Content()
        {
        }
        public Content(string name, ContentType type, uint amount, Package package)
        {
            Name = name.ToUpper();
            Type = type;
            Amount = amount;
            Package = package;
        }

        [Timestamp] // concurrency token property
        public byte[] RowVersion { get; set; }
    }
}
