using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Services;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Threading;

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

        private readonly DispatcherTimer _debounceTimer;

        public SendPackage()
        {
            InitializeComponent();

            DataContext = this;
            LocationsInfo.PropertyChanged += OnLocationsChanged;
            PackageInfo.PropertyChanged += OnLocationsChanged;

            _debounceTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void OnLocationsChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LocationInfoViewModel.FromLocation) ||
                e.PropertyName == nameof(LocationInfoViewModel.ToLocation) ||
                e.PropertyName == nameof(PackageInfoViewModel.Weight))
            {
                // Restart debounce timer
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }

        private async void DebounceTimer_Tick(object? sender, EventArgs e)
        {
            _debounceTimer.Stop();

            var from = LocationsInfo.FromLocation as Class_Lib.Warehouse;
            var to = LocationsInfo.ToLocation as Class_Lib.Warehouse;

            if (from != null && to != null && from.ID != to.ID && double.TryParse(PackageInfo.Weight, out _))
            {
                var route = await RouteService.GetRouteInfoAsync(
                    from.GeoData.Latitude, from.GeoData.Longitude,
                    to.GeoData.Latitude, to.GeoData.Longitude);

                if (route.IsSuccess)
                {
                    DeliveryCost = RouteService.CostEstimate(route.DistanceKm, double.Parse(PackageInfo.Weight)).ToString("F2") + " грн";
                    DeliveryTime = $"{route.Duration:hh\\:mm} год";
                }
                else
                {
                    DeliveryCost = "Помилка маршруту";
                    DeliveryTime = "-";
                }
            }
            else
            {
                DeliveryCost = "-";
                DeliveryTime = "-";
            }

            OnPropertyChanged(nameof(DeliveryCost));
            OnPropertyChanged(nameof(DeliveryTime));
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
                        ? height
                        : throw new ArgumentException("Висота повинна бути числом."),
                    double.TryParse(PackageInfo.Weight, out double weight)
                        ? weight
                        : throw new ArgumentException("Вага повинна бути числом."),
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

            if (route.IsSuccess) return RouteService.CostEstimate(route.DistanceKm, double.Parse(PackageInfo.Weight));
            else throw new Exception(route.ErrorMessage);
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                var repo = App.AppHost.Services.GetRequiredService<DeliveryMethods>();
                Delivery delivery = await CreateDelivery();
                await repo.AddAsync(App.CurrentEmployee, delivery);
                MessageBox.Show($"Номер доставки: {delivery.ID}\nНомер посилки: {delivery.PackageID}", "Доставку створено!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка при створенні доставки!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // to-do: clear all fields, might need a rewrite on how a lot of them are bound
        }
    }
}
