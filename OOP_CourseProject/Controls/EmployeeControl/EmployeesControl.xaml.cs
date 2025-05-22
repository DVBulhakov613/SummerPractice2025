using Class_Lib;
using Class_Lib.Backend.Person_related.Methods;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.Helpers;
using OOP_CourseProject.Controls.ViewModel;
using OOP_CourseProject.Controls.ViewModel.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Class_Lib.Backend.Serialization.DTO;

namespace OOP_CourseProject.Controls.EmployeeControl
{
    /// <summary>
    /// Interaction logic for EmployeesControl.xaml
    /// </summary>
    public partial class EmployeesControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public LocationInfoViewModel LocationsInfo { get; } = new();
        public EmployeeMethods EmployeeMethods { get; set; } = App.AppHost.Services.GetRequiredService<EmployeeMethods>();

        public async void GenerateViewModel()
        {
            var vm = ViewModelService.CreateViewModel(await EmployeeMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
            ViewModel = vm;
            DataContext = vm;
        }

        public EmployeesControl()
        {
            InitializeComponent();

            GenerateViewModel();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EmployeeDialog();
            if (dialog.ShowDialog() == true)
            {
                var newEmployee = dialog.Result;
 
                await EmployeeMethods.AddAsync(App.CurrentEmployee, newEmployee);
                RefreshButton_Click(sender, e);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem == null) return;
            var dialog = new EmployeeDialog((Employee)ViewModel.SelectedItem);
            if (dialog.ShowDialog() == true)
            {
                var updatedEmployee = dialog.Result;
                await EmployeeMethods.UpdateAsync(App.CurrentEmployee, updatedEmployee);
                RefreshButton_Click(sender, e);
            }
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem == null)
            {
                MessageBox.Show("Обраний об'єкт не знайдено.", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Видалити працівника {((Employee)ViewModel.SelectedItem).FullName}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                    await EmployeeMethods.DeleteAsync(App.CurrentEmployee, (Employee)ViewModel.SelectedItem);
                    RefreshButton_Click(sender, e);

            }

        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateItems(await EmployeeMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
        }

        private void SerilizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SerializationHelper.SerializeSelectedItemsToFolder<Employee>(EmployeesDataGrid, d => d.ToDto()))
                    MessageBox.Show("Файли збережені успішно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка. {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
