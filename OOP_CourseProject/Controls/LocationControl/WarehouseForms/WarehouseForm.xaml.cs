using Class_Lib;
using System.Windows;

namespace OOP_CourseProject.Controls.LocationControl.WarehouseForms
{
    /// <summary>
    /// Interaction logic for WarehouseForm.xaml
    /// </summary>
    public partial class WarehouseForm : Window
    {
        public WarehouseViewModel ViewModel { get; private set; }

        private Warehouse _originalModel;
        public Warehouse WarehouseResult { get; private set; }
        public WarehouseForm()
        {
            InitializeComponent();
            ViewModel = new WarehouseViewModel();
            DataContext = ViewModel;
        }

        public void SetData(Warehouse model)
        {
            _originalModel = model;
            ViewModel.LoadFromModel(model);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_originalModel != null)
                {
                    _originalModel.UpdateGeoData(new Coordinates(ViewModel.Longitude, ViewModel.Latitude, ViewModel.Address, ViewModel.Region));
                    _originalModel.MaxStorageCapacity = ViewModel.MaxStorageCapacity;
                    _originalModel.IsAutomated = ViewModel.IsAutomated;

                    WarehouseResult = _originalModel;
                }
                else
                {
                    WarehouseResult = ViewModel.ToModel();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}