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
        public string Name
        {
            get => _name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва не може бути пустою.");
                else
                    _name = value.ToUpper();
            }
        } // name given to content (cup, phone, etc)
        public ContentType Type { get; set; }// type of content (electronics, clothing, etc)
        public uint Amount { get; set; }
        public uint PackageID { get; private set; }
        public Package Package { get; set; } // the package that this content belongs to
        protected Content()
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
