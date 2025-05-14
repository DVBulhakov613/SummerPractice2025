using Class_Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class ContentViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = new ObservableCollection<InfoSection>();
        public ContentViewModel(Content content)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Назва", Value = $"{content.Name}" },
                    new() { Label = "Тип", Value = content.Type.ToString() },
                    new() { Label = "Кількість", Value = $"{content.Amount}"},
                    new() { Label = "Опис", Value = $"{content.Description}" }
                }
            });
        }
    }
}
