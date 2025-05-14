using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class InfoSectionList : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; }

        public InfoSectionList(string sectionTitle, IEnumerable<InfoItem> items)
        {
            InfoSections = new ObservableCollection<InfoSection>
            {
                new InfoSection
                {
                    SectionTitle = sectionTitle,
                    InfoItems = items.ToList()
                }
            };
        }
    }

}
