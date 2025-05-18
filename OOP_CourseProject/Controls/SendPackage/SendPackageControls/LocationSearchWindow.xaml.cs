using Class_Lib;
using Class_Lib.Backend.Database.Repositories;
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

namespace OOP_CourseProject.Controls.SendPackageControls
{
    /// <summary>
    /// Interaction logic for LocationSearchWindow.xaml
    /// </summary>
    public partial class LocationSearchWindow : Window
    {
        public LocationSearchWindow(Action<BaseLocation> onLocationSelected)
        {
            InitializeComponent();

            var repo = App.AppHost.Services.GetRequiredService<LocationRepository>();

            var viewModel = new DebouncedSearchViewModel<BaseLocation>(SearchFunction);
            viewModel.ItemSelected += location =>
            {
                onLocationSelected(location);
                this.Close();
            };

            DataContext = viewModel;

            async Task<IEnumerable<BaseLocation>> SearchFunction(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                    return Enumerable.Empty<BaseLocation>();

                return await repo.GetByCriteriaAsync(l =>
                    (!string.IsNullOrEmpty(l.GeoData.Address) && l.GeoData.Address.Contains(query)) ||
                    l.ID.ToString().Contains(query) ||
                    l.GeoData.Region.Contains(query));
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is BaseLocation selected)
            {
                var vm = (DebouncedSearchViewModel<BaseLocation>)DataContext;
                vm.SelectedItem = selected; // force-setting the selected item because the event might not have been triggered yet and i don't have the time to fix it, sorry!
                vm.Select();
            }
        }

    }
}

/*
var db = App.AppHost.Services.GetRequiredService<LocationRepository>();
var results = await db.GetByCriteriaAsync(l =>
    (!string.IsNullOrEmpty(l.GeoData.Address) && l.GeoData.Address.Contains(query)) ||
    l.ID.ToString().Contains(query) ||
    l.GeoData.Region.Contains(query)); 
*/