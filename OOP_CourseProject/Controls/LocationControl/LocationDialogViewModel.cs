using Class_Lib;
using GalaSoft.MvvmLight;
using OOP_CourseProject.Controls.LocationControl.PostalOfficeForms;
using OOP_CourseProject.Controls.LocationControl.WarehouseForms;
using OOP_CourseProject.Controls.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WarehouseViewModel = OOP_CourseProject.Controls.LocationControl.WarehouseForms.WarehouseViewModel;
using PostalOfficeViewModel = OOP_CourseProject.Controls.LocationControl.PostalOfficeForms.PostalOfficeViewModel;

namespace OOP_CourseProject.Controls.LocationControl
{
    public class LocationDialogViewModel : ViewModelBase
    {
        public object CurrentForm { get; private set; }
        public BaseLocation Result { get; private set; }

        public void SwitchTo(string type)
        {
            CurrentForm = null;
            RaisePropertyChanged(nameof(CurrentForm));

                switch (type)
                {
                    case "Warehouse":
                        var warehouseVm = new WarehouseViewModel();
                        CurrentForm = new WarehouseForm { DataContext = warehouseVm };
                        break;
                    case "PostalOffice":
                        var postVm = new PostalOfficeViewModel();
                        CurrentForm = new PostalOfficeForm { DataContext = postVm };
                        break;
                }

                RaisePropertyChanged(nameof(CurrentForm));
        }


        public void Confirm()
        {
            switch (CurrentForm)
            {
                case WarehouseForm wf when wf.DataContext is WarehouseForms.WarehouseViewModel wvm:
                    Result = wvm.ToModel(); break;
                case PostalOfficeForm pf when pf.DataContext is PostalOfficeForms.PostalOfficeViewModel pvm:
                    Result = pvm.ToModel(); break;
            }
        }
    }

}
