using Class_Lib;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Serialization.DTO;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.Helpers;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls.ReceivePackageControls
{
    /// <summary>
    /// Interaction logic for ReceivePackageControl.xaml
    /// </summary>
    public partial class ReceivePackageControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public DeliveryMethods DeliveryMethods { get; } = App.AppHost.Services.GetRequiredService<DeliveryMethods>();

        public ReceivePackageControl()
        {
            InitializeComponent();
            GenerateViewModel();
        }

        public async void GenerateViewModel()
        {
            if (App.CurrentEmployee.Employee.Workplace == null && !App.CurrentEmployee.CachedPermissions.Contains(2))
            {
                MessageBox.Show("Ви не прив'язані до жодного відділення. Пакунки для отримання не доступні.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if(App.CurrentEmployee.Employee.Workplace == null && App.CurrentEmployee.CachedPermissions.Contains(2))
            {
                MessageBox.Show("Ви не прив'язані до жодного відділення. Показано доставки з усіх відділень.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                var packages = await DeliveryMethods.GetByCriteriaAsync(App.CurrentEmployee,
                    p => p.Package.PackageStatus == PackageStatus.IN_TRANSIT || p.Package.PackageStatus == PackageStatus.RECEIVED
                );
                ViewModel = ViewModelService.CreateViewModel(packages);
                DataContext = ViewModel;
            }
            else
            {
                var packages = await DeliveryMethods.GetByCriteriaAsync(App.CurrentEmployee,
                    p => p.SentToID == App.CurrentEmployee.Employee.WorkplaceID
                    && (p.Package.PackageStatus == PackageStatus.IN_TRANSIT || p.Package.PackageStatus == PackageStatus.RECEIVED)
                );
                ViewModel = ViewModelService.CreateViewModel(packages);
                DataContext = ViewModel;
            }
        }

        private async void ReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not Delivery delivery || delivery.Package.PackageStatus != PackageStatus.IN_TRANSIT)
                return;

            delivery.Package.PackageStatus = PackageStatus.RECEIVED;
            await DeliveryMethods.UpdateAsync(App.CurrentEmployee, delivery);
            RefreshButton_Click(sender, e);
        }

        private async void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not Delivery delivery || delivery.Package.PackageStatus != PackageStatus.RECEIVED)
                return;

            delivery.Package.PackageStatus = PackageStatus.DELIVERED;
            await DeliveryMethods.UpdateAsync(App.CurrentEmployee, delivery);
            RefreshButton_Click(sender, e);
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var deliveries = await DeliveryMethods.GetByCriteriaAsync(
                App.CurrentEmployee,
                p => p.SentToID == App.CurrentEmployee.Employee.Workplace.ID &&
                     (p.Package.PackageStatus == PackageStatus.IN_TRANSIT || p.Package.PackageStatus == PackageStatus.RECEIVED)
            );
            ViewModel.UpdateItems(deliveries);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SerializationHelper.SerializeSelectedItemsToFolder<Package>(PackageDataGrid, p => p.ToDto()))
                    MessageBox.Show("Файли збережені успішно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка. {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
