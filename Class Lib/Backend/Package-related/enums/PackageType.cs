using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public enum PackageType
    {
        [Description("Стандартна")]
        Standard = 1,
        [Description("Поштова")]
        File,
        [Description("Палетна")]
        Palette
    }
}
