using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public interface IHasIdentification
    {
        public uint ID { get; } // unique identifier for the object
    }
}
