using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib
{
    public enum ContentType
    {
        [Description("Документи")]
        Documents,

        [Description("Електроніка")]
        Electronics,

        [Description("Одяг")]
        Clothes,

        [Description("Фурнітура")]
        Furniture,

        [Description("Іграшки")]
        Toys,

        [Description("Медичні препарати")]
        Medicine,

        [Description("Книги")]
        Books,

        [Description("Інструменти")]
        Tools,

        [Description("Продукти харчування")]
        Perishable,

        [Description("Крихкі")]
        Fragile,

        [Description("Небезпечні")]
        Hazardous,

        [Description("Цінні")]
        Valuables,

        [Description("Інше")]
        Miscellaneous
    }
}
