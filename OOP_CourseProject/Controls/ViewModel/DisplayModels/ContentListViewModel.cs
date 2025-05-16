using Class_Lib;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel.DisplayModels
{
    public class ContentListViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public ContentListViewModel(IEnumerable<Content> content)
        {
            foreach (Content c in content)
            {
                InfoSections.Add(new InfoSection
                {
                    SectionTitle = $"{c.Name} #{c.PackageID}",
                    InfoItems = new List<InfoItem>
                    {
                        new() { Label = "Назва: ", Value = c.Name },
                        new() { Label = "Тип: ", Value = c.Type.ToString() },
                        new() { Label = "Кількість: ", Value = c.Amount.ToString() },
                        new() { Label = "Опис: ", Value = string.IsNullOrEmpty(c.Description) ? "Відсутній" : c.Description}
                    }
                });
            }
        }
    }
}
