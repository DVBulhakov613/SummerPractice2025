using Class_Lib;
using Class_Lib.Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public static class ViewModelService
    {
        public static GenericInfoDisplayViewModel CreateViewModel(IEnumerable<object> models)
        {
            return new GenericInfoDisplayViewModel(models, ChooseViewModel);
        }

        private static IInfoProviderViewModel ChooseViewModel(object model)
        {
            return model switch
            {
                Package p => new PackageInfoViewModel(p),
                //Employee e => new EmployeeInfoViewModel(e),
                _ => throw new NotSupportedException($"Unsupported model type: {model.GetType().Name}")
            };
        }
    }
}
