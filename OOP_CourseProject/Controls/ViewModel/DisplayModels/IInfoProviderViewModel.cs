using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public interface IInfoProviderViewModel
    {
        ObservableCollection<InfoSection> InfoSections { get; }
    }

}
