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
                        Value = employee.WorkplaceID != null ? employee.WorkplaceID.ToString() : "Невідомо" 
                    }
                }
            });
        }
    }

}
