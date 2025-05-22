using Class_Lib;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP_CourseProject.Controls.LocationControl
{
    /// <summary>
    /// Interaction logic for EmployeeSearchWindow.xaml
    /// </summary>
    public partial class EmployeeSearchWindow : Window
    {
        public EmployeeSearchWindow(List<Employee> initiallyAssigned, Action<List<Employee>> onEmployeesSelected)
        {
            InitializeComponent();

            var repo = App.AppHost.Services.GetRequiredService<EmployeeMethods>();
            var viewModel = new EmployeeSearchViewModel(SearchFunction, initiallyAssigned);

            viewModel.Confirmed += employees =>
            {
                onEmployeesSelected(employees);
                Close();
            };

            DataContext = viewModel;

            async Task<IEnumerable<Employee>> SearchFunction(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                    return Enumerable.Empty<Employee>();

                return await repo.GetByCriteriaAsync(
                    App.CurrentEmployee,
                    e =>
                        e.FirstName.Contains(query) ||
                        e.Surname.Contains(query) ||
                        e.Email.Contains(query) ||
                        e.ID.ToString().Contains(query));
            }
        }


        private void SearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Employee employee)
            {
                var vm = (EmployeeSearchViewModel)DataContext;
                vm.AddEmployee(employee);
            }
        }
    }
}