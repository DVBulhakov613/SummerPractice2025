using Class_Lib.Backend.Database.Repositories;
using Class_Lib;
using Class_Lib.Backend.Person_related;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Shapes;
using Class_Lib.Database.Repositories;

namespace OOP_CourseProject.Controls.SendPackageControls
{
    /// <summary>
    /// Interaction logic for ClientSearchWindow.xaml
    /// </summary>
    public partial class ClientSearchWindow : Window
    {
        //public ClientSearchWindow(Action<Client> onClientSelected)
        //{
        //    InitializeComponent();

        //    var viewModel = new ClientSearchViewModel();
        //    viewModel.ClientSelected += client =>
        //    {
        //        onClientSelected(client);
        //        this.Close();
        //    };

        //    DataContext = viewModel;
        //}

        //private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (DataContext is ClientSearchViewModel vm && vm.SelectedClient != null)
        //    {
        //        vm.SelectCommand.Execute(null);
        //    }
        //}

        public ClientSearchWindow(Action<Client> onLocationSelected)
        {
            InitializeComponent();

            var repo = App.AppHost.Services.GetRequiredService<ClientRepository>();

            var viewModel = new DebouncedSearchViewModel<Client>(SearchFunction);
            viewModel.ItemSelected += location =>
            {
                onLocationSelected(location);
                this.Close();
            };

            DataContext = viewModel;

            async Task<IEnumerable<Client>> SearchFunction(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                    return Enumerable.Empty<Client>();

                return await repo.GetByCriteriaAsync(client =>
                    client.ID.ToString().Contains(query) ||
                    client.FirstName.Contains(query) ||
                    client.Surname.Contains(query) ||
                    client.Email.Contains(query) ||
                    client.PhoneNumber.Contains(query));
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is Client selected)
            {
                var vm = (DebouncedSearchViewModel<Client>)DataContext;
                vm.SelectedItem = selected; // force-setting the selected item because the event might not have been triggered yet and i don't have the time to fix it, sorry!
                vm.Select();
            }
        }
    }
}
