using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class WarehouseViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public WarehouseViewModel(Warehouse wh)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Ідентефікаційний код", Value = $"{wh.ID}" },
                    new() { Label = "Адреса", Value = wh.GeoData.Address ?? "Невідома"},
                    new() { Label = "Регіон", Value = wh.GeoData.Region ?? "Невідомий" },
                    new() { Label = "Тип", Value = wh.LocationType },
                    //new() { Label = "Працівники: ", Value = po.Staff },
                    //new() { Label = "Посада", Value = po.Role.Name },
                }
            });

            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Специфічна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Тип", Value = wh.LocationType },
                    new() { Label = "Автоматизований?", Value = wh.IsAutomated ? "Так" : "Ні" },
                    new() { Label = "Повний?", Value = wh.IsFull ? "Так" : "Ні" },
                    new InfoItem
                    {
                        Label = "Працівники",
                        Value = wh.Staff == null || wh.Staff.Count == 0 ? "Відсутні" : "Список...",
                        OnClick = wh.Staff == null || wh.Staff.Count == 0 ? null : () =>
                        {
                            var staffViewModel = new StaffListViewModel(wh.Staff); // must implement IInfoProviderViewModel
                            var window = new InfoPopupWindow(staffViewModel);
                            window.ShowDialog();
                        }
                    }
                    //new() { Label = "Посада", Value = po.Role.Name },
                }
            });
        }
    }
}
