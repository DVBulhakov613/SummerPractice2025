using Class_Lib.Database.Repositories;
using Class_Lib;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using OOP_CourseProject.Controls.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Location_related.Methods;

namespace OOP_CourseProject.Controls
{
    public partial class HomePageControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public HomePageControl()
        {
            InitializeComponent();

            GenerateViewModel();
        }

        public async void GenerateViewModel()
        {
            var repo = App.AppHost.Services.GetRequiredService<LocationMethods>();

            DataContext = ViewModelService.CreateViewModel(await repo.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
        }
    }
}