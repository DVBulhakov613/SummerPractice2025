using Class_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public interface ILocationViewModelFactory
    {
        IInfoProviderViewModel Create(BaseLocation location);
    }
}
