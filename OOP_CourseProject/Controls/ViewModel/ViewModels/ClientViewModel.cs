using Class_Lib;
using Class_Lib.Backend.Person_related;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class ClientViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public ClientViewModel(Client client)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Повне ім'я", Value = $"{client.FullName}" },
                    new() { Label = "Телефон", Value = client.PhoneNumber },
                    new() { Label = "Email", Value = client.Email }
                }
            });
        }
    }
}
