using Class_Lib;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.ClientControl.ClientControls;
using OOP_CourseProject.Controls.ViewModel;
using OOP_CourseProject.Controls.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}

/*

        private void AddEmployee()
        {
            var dialog = new EmployeeDialog();
            if (dialog.ShowDialog() == true)
            {
                var newEmployee = dialog.Result;
                Task.Run(async () =>
                {
                    await _employeeRepository.AddAsync(newEmployee);
                    await LoadEmployeesAsync();
                });
            }
        }

        private void UpdateEmployee()
        {
            if (SelectedItem == null) return;
            var dialog = new EmployeeDialog(SelectedItem);
            if (dialog.ShowDialog() == true)
            {
                var updatedEmployee = dialog.Result;
                Task.Run(async () =>
                {
                    await _employeeRepository.UpdateAsync(updatedEmployee);
                    await LoadEmployeesAsync();
                });
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedItem == null) return;
            var result = MessageBox.Show($"Видалити працівника {SelectedItem.FullName}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Task.Run(async () =>
                {
                    await _employeeRepository.DeleteAsync(SelectedItem);
                    await LoadEmployeesAsync();
                });
            }
        }

*/
