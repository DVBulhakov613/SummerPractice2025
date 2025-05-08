using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.ViewModels
{
    public class InfoSection
    {
        public string SectionTitle { get; set; }
        public List<InfoItem> InfoItems { get; set; } = new();
    }
}
