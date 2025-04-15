using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public interface ICustomerFacing
    {
        bool HandlesPublicDropOffs { get; set; } // indicates if the location handles public drop-offs
        bool IsRegionalHQ { get; set; } // indicates if the location is a regional headquarters
    }
}
