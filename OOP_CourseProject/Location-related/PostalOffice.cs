using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject
{
    public class PostalOffice : BaseLocation
    {

        public bool HandlesPublicDropOffs { get; set; }
        public bool IsRegionalHQ { get; set; }
    }
}
