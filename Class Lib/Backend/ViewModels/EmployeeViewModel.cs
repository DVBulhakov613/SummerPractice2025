using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.ViewModels
{
    public class EmployeeViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = new();

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
