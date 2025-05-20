using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.ClientControl.ClientControls;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls
{
    /// <summary>
    /// Interaction logic for ClientsControl.xaml
    /// </summary>
    public partial class ClientsControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public ClientMethods ClientMethods { get; set; } = App.AppHost.Services.GetRequiredService<ClientMethods>();

        public async void GenerateViewModel()
        {
            var vm = ViewModelService.CreateViewModel(await ClientMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
            ViewModel = vm;
            DataContext = vm;
        }

        public ClientsControl()
        {
            InitializeComponent();

            GenerateViewModel();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ClientDialog();
            if (dialog.ShowDialog() == true)
            {
                await App.AppHost.Services.GetRequiredService<ClientMethods>().AddAsync(App.CurrentEmployee, dialog.Result);
            }
            RefreshButton_Click(sender, e);
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem == null) return;
            var dialog = new ClientDialog((Client)ViewModel.SelectedItem);
            if (dialog.ShowDialog() == true)
            {
                var updatedClient = dialog.Result;
                await ClientMethods.UpdateAsync(App.CurrentEmployee, updatedClient);
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
            var result = MessageBox.Show($"Видалити клієнта {((Client)ViewModel.SelectedItem).FullName}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await ClientMethods.DeleteAsync(App.CurrentEmployee, (Client)ViewModel.SelectedItem);
                RefreshButton_Click(sender, e);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateItems(await ClientMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
        }
    }
}

/*
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
            if (ViewModel.SelectedItem == null) return;
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
*/