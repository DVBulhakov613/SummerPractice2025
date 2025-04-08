using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Package_related
{
    public class Content
    {
        public string Name { get; init; }
        public ContentType Type { get; init; }
        public int Amount { get; init; }
        public Content(string name, ContentType type, int amount)
        {
            Name = name.ToUpper();
            Type = type;
            Amount = amount;
        }
    }
}
