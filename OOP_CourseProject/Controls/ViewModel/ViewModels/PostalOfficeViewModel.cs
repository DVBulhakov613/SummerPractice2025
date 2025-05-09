using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class PostalOfficeViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public PostalOfficeViewModel(PostalOffice po)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Ідентефікаційний код", Value = $"{po.ID}" },
                    new() { Label = "Адреса", Value = po.GeoData.Address != null ? po.GeoData.Address : "Невідома"},
                    new() { Label = "Регіон", Value = po.GeoData.Region != null ? po.GeoData.Region : "Невідомий" },
                    new() { Label = "Тип", Value = po.LocationType },
                    //new() { Label = "Працівники: ", Value = po.Staff },
                    //new() { Label = "Посада", Value = po.Role.Name },
                }
            });

            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Специфічна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Тип", Value = po.LocationType },
                    new() { Label = "Публічний?", Value = po.HandlesPublicDropOffs ? "Так" : "Ні" },
                    new() { Label = "Автоматизований?", Value = po.IsAutomated ? "Так" : "Ні" },
                    new() { Label = "Регіональний штаб?", Value = po.IsRegionalHQ ? "Так" : "Ні" },
                    new() { Label = "Повний?", Value = po.IsFull ? "Так" : "Ні" },
                    //new()
                    //{
                    //    Label = "Працівники",
                    //    Value = po.Staff != null && po.Staff.Any()
                    //        ? string.Join(", ", po.Staff.Select(e => $"{e.FirstName} {e.Surname}"))
                    //        : "Немає працівників"
                    //}

                    new InfoItem
                    {
                        Label = "Працівники",
                        Value = po.Staff == null || po.Staff.Count() == 0 ? "Відсутні" : "Список...",
                        OnClick = po.Staff == null || po.Staff.Count() == 0 ? null : () =>
                        {
                            var staffViewModel = new StaffListViewModel(po.Staff); // must implement IInfoProviderViewModel
                            var window = new InfoPopupWindow(staffViewModel);
                            window.ShowDialog();
                        }
                    }
                }
            });
        }
    }
}

/*
public uint ID { get; private set; } // could have either -1 or 0 for an undefined location, otherwise positive integers
public Coordinates GeoData { get; private set; } // location related data (coordinates, address, etc.)
public List<Employee>? Staff { get; set; } = new(); // staff assigned to this location

*/