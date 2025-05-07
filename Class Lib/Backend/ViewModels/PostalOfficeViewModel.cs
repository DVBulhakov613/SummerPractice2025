using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.ViewModels
{
    public class PostalOfficeViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = new();

        public PostalOfficeViewModel(PostalOffice po)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Повне ім'я", Value = $"{po.FullName}" },
                    new() { Label = "Телефон", Value = po.PhoneNumber },
                    new() { Label = "Email", Value = po.Email },
                    new() { Label = "Посада", Value = po.Role.Name },
                }
            });
        }
    }
}
