//using Class_Lib;
//using Class_Lib.Backend.Services;
//using Microsoft.EntityFrameworkCore.Metadata.Conventions;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OOP_CourseProject.Controls.ViewModel.DisplayModels
//{
//    public class RoleViewModel : IInfoProviderViewModel
//    {
//        public ObservableCollection<InfoSection> InfoSections { get; } = [];

//        public RoleViewModel(Role role)
//        {
//            InfoSections.Add(new InfoSection
//            {
//                SectionTitle = "Загальна інформація",
//                InfoItems = new List<InfoItem>
//                {
//                    new() { Label = "ID", Value = role.ID.ToString() },
//                    new() { Label = "Назва", Value = role.Name }
//                }
//            });

//            InfoSections.Add(new InfoSection
//            {
//                SectionTitle = "Специфічна інформація",
//                InfoItems = new List<InfoItem>
//                {
                    

//                    new InfoItem
//                    {
//                        Label = "Працівники",
//                        Value = role.Staff == null || role.Staff.Count == 0 ? "Відсутні" : "Список...",
//                        OnClick = role.Staff == null || role.Staff.Count == 0 ? null : () =>
//                        {
//                            var staffViewModel = new StaffListViewModel(role.Staff); // must implement IInfoProviderViewModel
//                            var window = new InfoPopupWindow(staffViewModel);
//                            window.ShowDialog();
//                        }
//                    }
//                }
//            });
//        }
//    }
//}
