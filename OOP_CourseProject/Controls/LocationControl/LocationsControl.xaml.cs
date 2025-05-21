using Class_Lib;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Serialization.DTO;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.Helpers;
using OOP_CourseProject.Controls.LocationControl;
using OOP_CourseProject.Controls.LocationControl.PostalOfficeForms;
using OOP_CourseProject.Controls.LocationControl.WarehouseForms;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;
using PostalOfficeViewModel = OOP_CourseProject.Controls.LocationControl.PostalOfficeForms.PostalOfficeViewModel;
using WarehouseViewModel = OOP_CourseProject.Controls.LocationControl.WarehouseForms.WarehouseViewModel;

namespace OOP_CourseProject.Controls
{
    /// <summary>
    /// Interaction logic for LocationControl.xaml
    /// </summary>
    public partial class LocationsControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public LocationMethods LocationMethods { get; set; } = App.AppHost.Services.GetRequiredService<LocationMethods>();

        public LocationsControl()
        {
            InitializeComponent();
            Loaded += LocationsControl_Loaded;
        }

        private async void LocationsControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ViewModelService.CreateViewModel(await LocationMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
            DataContext = ViewModel;
        }



        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var selector = new LocationTypeSelectionWindow();
            if (selector.ShowDialog() != true) return;

            BaseLocation newLocation = null;

            switch (selector.SelectedLocationType)
            {
                case "Warehouse":
                    var warehouseForm = new WarehouseForm();
                    if (warehouseForm.ShowDialog() == true)
                        newLocation = warehouseForm.WarehouseResult;
                    break;

                case "PostalOffice":
                    var poForm = new PostalOfficeForm();
                    if (poForm.ShowDialog() == true)
                        newLocation = poForm.PostalOfficeResult;
                    break;
            }

            if (newLocation != null)
            {
                await LocationMethods.AddAsync(App.CurrentEmployee, newLocation);
            }
        }


        //static private LocationType? ShowLocationTypeSelector()
        //{
        //    var result = MessageBox.Show("Додати склад? Натисніть 'Yes' для складу, 'No' для поштового відділення.",
        //                                 "Тип локації",
        //                                 MessageBoxButton.YesNoCancel);

        //    return result switch
        //    {
        //        MessageBoxResult.Yes => LocationType.Warehouse,
        //        MessageBoxResult.No => LocationType.PostalOffice,
        //        _ => null
        //    };
        //}


        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not BaseLocation selected)
                return;

            BaseLocation updatedLocation = null;

            if (selected is PostalOffice office)
            {
                var poForm = new PostalOfficeForm();
                poForm.SetData(office);
                if (poForm.ShowDialog() == true)
                {
                    updatedLocation = poForm.PostalOfficeResult;
                }
            }
            else if (selected is Warehouse warehouse)
            {
                var whForm = new WarehouseForm();
                whForm.SetData(warehouse);
                if (whForm.ShowDialog() == true)
                {
                    updatedLocation = whForm.WarehouseResult;
                }
            }


            if (updatedLocation != null)
            {
                await LocationMethods.UpdateAsync(App.CurrentEmployee, updatedLocation);
                await RefreshAsync();
            }
        }



        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem == null)
            {
                MessageBox.Show("Обраний об'єкт не знайдено.", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Видалити відділення {((BaseLocation)ViewModel.SelectedItem).ID}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await LocationMethods.DeleteAsync(App.CurrentEmployee, (BaseLocation)ViewModel.SelectedItem);
                RefreshButton_Click(sender, e);
            }
        }

        private async Task RefreshAsync()
        {
            ViewModel.UpdateItems(await LocationMethods.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }


        private void SerilizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SerializationHelper.SerializeSelectedItemsToFolder<BaseLocation>(LocationsDataGrid, d => d.ToDto()))
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
