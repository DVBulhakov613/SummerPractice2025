using Class_Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class StaffListViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public StaffListViewModel(IEnumerable<Employee> staff)
        {
            foreach (var emp in staff)
            {
                InfoSections.Add(new InfoSection
                {
                    SectionTitle = $"{emp.FullName} #{emp.ID}",
                    InfoItems = new List<InfoItem>
                    {
                        new() { Label = "Ідентефікаційний код", Value = $"{emp.ID}" },
                        new() { Label = "Ім’я", Value = emp.FullName },
                        new() { Label = "Телефон", Value = emp.PhoneNumber },
                        new() { Label = "Email", Value = emp.Email },
                        new() { Label = "Посада", Value = emp.Role?.Name ?? "Невідомо" }
                    }
                });
            }
        }
    }
}
