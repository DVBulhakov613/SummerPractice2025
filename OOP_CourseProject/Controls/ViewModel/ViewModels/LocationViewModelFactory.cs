using Class_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class LocationViewModelFactory : ILocationViewModelFactory
    {
        public IInfoProviderViewModel Create(BaseLocation location)
        {
            return location switch
            {
                PostalOffice po => new PostalOfficeViewModel(po),
                Warehouse wh => new WarehouseViewModel(wh),
                _ => throw new NotSupportedException($"Unsupported location type: {location.GetType().Name}")
            };
        }
    }

}
