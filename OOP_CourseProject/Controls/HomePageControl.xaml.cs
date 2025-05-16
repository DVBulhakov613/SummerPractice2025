using Class_Lib.Database.Repositories;
using Class_Lib;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using OOP_CourseProject.Controls.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Person_related.Methods;
using System.Threading.Tasks;
using System.Xml;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Database;

namespace OOP_CourseProject.Controls
{
    public partial class HomePageControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public DeliveryMethods DeliveryMethods { get; set; } = App.AppHost.Services.GetRequiredService<DeliveryMethods>();
        public HomePageControl()
        {
            InitializeComponent();

            GenerateViewModel();
        }

        public async void GenerateViewModel()
        {
            var vm = ViewModelService.CreateViewModel(await DeliveryMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
            ViewModel = vm;
            DataContext = vm;
        }

        private void DelayedPackages_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FindPackage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Information_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Announcements_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchSpecific_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateItems(await DeliveryMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            var delivery = ViewModel.SelectedItem as Delivery;
            if (delivery is null)
            {
                MessageBox.Show("Обраний об'єкт не знайдено.", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var db = App.AppHost.Services.GetRequiredService<DeliveryMethods>();

            await db.DeleteAsync(App.CurrentEmployee, delivery);

            Refresh_Click(sender, e);
        }
    }
}