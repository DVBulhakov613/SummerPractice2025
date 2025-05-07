using Class_Lib;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls
{
    public partial class EmployeeControl : UserControl
    {
        private readonly EmployeeMethods _employeeMethods;

        public EmployeeControl(EmployeeMethods employeeMethods)
        {
            InitializeComponent();
            _employeeMethods = employeeMethods;

            LoadEmployees();
        }

        private async void LoadEmployees()
        {
            try
            {
                var employees = await _employeeMethods.GetEmployeesByCriteriaAsync(null, e => true); // Load all employees
                EmployeeTable.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}");
            }
        }

        private async void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            // to do (probably with another window / user control, which will then parse an object here)
        }

        private async void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeTable.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                // to do
            }
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeTable.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {selectedEmployee.FirstName} {selectedEmployee.Surname}?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _employeeMethods.DeleteEmployeeAsync(App.CurrentEmployee, selectedEmployee);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }
                    finally
                    {
                        LoadEmployees();
                    }
                }
            }
        }

        // to do
        private async void PromoteToManager_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeTable.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                try
                {
                    await _employeeMethods.PromoteToManagerAsync(App.CurrentEmployee, selectedEmployee, new List<BaseLocation> { selectedEmployee.Workplace });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
                finally
                {
                    LoadEmployees();
                }
            }
        }

        private async void PromoteToAdministrator_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeTable.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                try
                {
                    await _employeeMethods.PromoteToAdministratorAsync(App.CurrentEmployee, selectedEmployee);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
                finally
                {
                    LoadEmployees();
                }
            }
        }
    }
}

