using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Class_Lib.Backend.ViewModels
{
    /// <summary>
    /// Represents a single item in the info panel.
    /// </summary>
    public class InfoItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public Action? OnClick { get; set; }
        public bool IsClickable => OnClick != null;
    }


}
