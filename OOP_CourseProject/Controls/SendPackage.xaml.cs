using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Services;
using OOP_CourseProject.Controls.SendPackageControls;
using OOP_CourseProject.Controls.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace OOP_CourseProject.Controls
{
    /// <summary>
    /// Interaction logic for SendPackage.xaml
    /// </summary>
    public partial class SendPackage : UserControl, INotifyPropertyChanged
    {
        public ClientInfoViewModel ClientInfo { get; } = new();
        public LocationInfoViewModel LocationsInfo { get; } = new();
        public PackageInfoViewModel PackageInfo { get; } = new();
        public ContentInfoViewModel ContentInfo { get; } = new();

        public string DeliveryCost { get; set; } = "-";
        public string DeliveryTime { get; set; } = "-";
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public SendPackage()
        {
            InitializeComponent();

            DataContext = this;
            LocationsInfo.PropertyChanged += OnLocationsChanged;
        }

        private async void OnLocationsChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LocationInfoViewModel.FromLocation) ||
                e.PropertyName == nameof(LocationInfoViewModel.ToLocation))
            {
                var from = LocationsInfo.FromLocation as Class_Lib.Warehouse;
                var to = LocationsInfo.ToLocation as Class_Lib.Warehouse;

                if (from != null && to != null && from.ID != to.ID)
                {
                    var route = await RouteService.GetRouteInfoAsync(
                        from.GeoData.Latitude, from.GeoData.Longitude,
                        to.GeoData.Latitude, to.GeoData.Longitude);

                    if (route.IsSuccess)
                    {
                        DeliveryCost = RouteService.CostEstimate(route.DistanceKm).ToString("F2") + " грн";
                        DeliveryTime = $"{route.Duration:hh\\:mm} год";
                    }
                    else
                    {
                        DeliveryCost = "Помилка маршруту";
                        DeliveryTime = "-";
                    }

                    OnPropertyChanged(nameof(DeliveryCost));
                    OnPropertyChanged(nameof(DeliveryTime));
                }
                else
                {
                    DeliveryCost = "-";
                    DeliveryTime = "-";
                    OnPropertyChanged(nameof(DeliveryCost));
                    OnPropertyChanged(nameof(DeliveryTime));
                }
            }
        }


        private async Task<Delivery> CreateDelivery()
        {
            Delivery delivery = new Delivery(
                new Package(
                    uint.TryParse(PackageInfo.Length, out uint length) 
                        ? length
                        : throw new ArgumentException("Довжина повинна бути числом."),
                    uint.TryParse(PackageInfo.Width, out uint width)
                        ? width
                        : throw new ArgumentException("Ширина повинна бути числом."),
                    uint.TryParse(PackageInfo.Height, out uint height)
                        ? width
                        : throw new ArgumentException("Ширина повинна бути числом."),
                    double.TryParse(PackageInfo.Weight, out double weight)
                        ? weight
                        : throw new ArgumentException("Ширина повинна бути числом."),
                    ClientInfo.Sender,
                    ClientInfo.Receiver,
                    (Class_Lib.Warehouse)LocationsInfo.FromLocation,
                    (Class_Lib.Warehouse)LocationsInfo.ToLocation,
                    PackageInfo.SelectedPackageType
                    ),
                ClientInfo.Sender,
                ClientInfo.Receiver,
                (Class_Lib.Warehouse)LocationsInfo.FromLocation,
                (Class_Lib.Warehouse)LocationsInfo.ToLocation,
                await CostEstimate(),
                false);

            foreach(ContentItem item in ContentInfo.Items)
            {
                if (item is null) continue;
                delivery.Package.AddContent(item.Name, item.ContentType, item.Amount, item.Description);
            }

            return delivery;
        }

        public async Task<double> CostEstimate()
        {
            RouteInfo route = await RouteService.GetRouteInfoAsync(
                ((Class_Lib.Warehouse)LocationsInfo.FromLocation).GeoData.Latitude,
                ((Class_Lib.Warehouse)LocationsInfo.FromLocation).GeoData.Longitude,
                ((Class_Lib.Warehouse)LocationsInfo.ToLocation).GeoData.Latitude,
                ((Class_Lib.Warehouse)LocationsInfo.ToLocation).GeoData.Longitude);

            if (route.IsSuccess) return RouteService.CostEstimate(route.DistanceKm);
            else throw new Exception(route.ErrorMessage);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
