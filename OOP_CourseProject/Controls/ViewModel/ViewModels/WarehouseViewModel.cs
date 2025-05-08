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
        public ObservableCollection<InfoSection> InfoSections { get; } = new();

        public WarehouseViewModel(Warehouse wareh)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Ідентефікаційний код", Value = $"{wareh.ID}" },
                    new() { Label = "Адреса", Value = wareh.GeoData.Address != null ? wareh.GeoData.Address : "Невідома"},
                    new() { Label = "Регіон", Value = wareh.GeoData.Region != null ? wareh.GeoData.Region : "Невідомий" },
                    new() { Label = "Тип", Value = wareh.LocationType },
                    //new() { Label = "Працівники: ", Value = po.Staff },
                    //new() { Label = "Посада", Value = po.Role.Name },
                }
            });

            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Специфічна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Тип", Value = wareh.LocationType },
                    new() { Label = "Автоматизований?", Value = wareh.IsAutomated ? "Так" : "Ні" },
                    new() { Label = "Повний?", Value = wareh.IsFull ? "Так" : "Ні" }
                    //new() { Label = "Працівники: ", Value = po.Staff },
                    //new() { Label = "Посада", Value = po.Role.Name },
                }
            });
        }
    }
}
