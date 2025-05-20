using Class_Lib;
using Class_Lib.Database.Repositories;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OOP_CourseProject.Controls.EmployeeControl
{
    public class EmployeesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Items { get; } = new();

        private Employee _selectedItem;
        public Employee SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        private readonly EmployeeRepository _employeeRepository;

        public EmployeesViewModel()
        {
            _employeeRepository = App.AppHost.Services.GetRequiredService<EmployeeRepository>();

            AddCommand = new RelayCommand(AddEmployee);
            UpdateCommand = new RelayCommand(UpdateEmployee, () => SelectedItem != null);
            DeleteCommand = new RelayCommand(DeleteEmployee, () => SelectedItem != null);
            RefreshCommand = new RelayCommand(async () => await LoadEmployeesAsync());

            Task.Run(async () => await LoadEmployeesAsync());
        }

        private async Task LoadEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Items.Clear();
                foreach (var employee in employees)
                    Items.Add(employee);
            });
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
