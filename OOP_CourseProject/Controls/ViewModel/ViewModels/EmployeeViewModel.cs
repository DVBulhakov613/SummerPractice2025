using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class EmployeeViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public EmployeeViewModel(Employee employee)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Ідентефікаційний код", Value = $"{employee.ID}" },
                    new() { Label = "Повне ім'я", Value = $"{employee.FullName}" },
                    new() { Label = "Телефон", Value = employee.PhoneNumber },
                    new() { Label = "Email", Value = employee.Email },
                    new() { Label = "Посада", Value = employee.Role.Name },
                }
            });

            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Інформація компанії",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Посада", Value = $"{employee.FullName}" },
                    new() { 
                        Label = "Місце працевлаштування", 
                        Value = employee.WorkplaceID != null ? employee.WorkplaceID.ToString() : "Невідомо",
                        OnClick = employee.Workplace == null ? null : () =>
                        {
                            switch(employee.Workplace)
                            {
                                case PostalOffice po:
                                    var postalOfficeViewModel = new PostalOfficeViewModel(po);
                                    var window = new InfoPopupWindow(postalOfficeViewModel);
                                    window.ShowDialog();
                                    break;
                                case Warehouse wh:
                                    var warehouseViewModel = new WarehouseViewModel(wh);
                                    var window2 = new InfoPopupWindow(warehouseViewModel);
                                    window2.ShowDialog();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            });
        }
    }

}
