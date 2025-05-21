using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Serialization.DTO;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.Helpers;
using OOP_CourseProject.Controls.ViewModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

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
            try
            {
                if(SerializationHelper.SerializeSelectedItemsToFolder<Delivery>(DataGrid, d => d.ToDto()))
                    MessageBox.Show("Файли збережені успішно.");
            }
            catch(Exception ex) {                 
                MessageBox.Show($"Помилка. {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
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

            await DeliveryMethods.DeleteAsync(App.CurrentEmployee, delivery);

            Refresh_Click(sender, e);
        }
    }
}