using Class_Lib;
using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace OOP_CourseProject.Controls.ReceivePackageControls
{
    /// <summary>
    /// Interaction logic for PackageLogControl.xaml
    /// </summary>
    public partial class PackageLogControl : UserControl
    {
        public PackageLogControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty DeliveryProperty =
            DependencyProperty.Register(nameof(Delivery), typeof(Delivery), typeof(PackageLogControl),
                new PropertyMetadata(null, OnDeliveryChanged));

        public Delivery Delivery
        {
            get => (Delivery)GetValue(DeliveryProperty);
            set => SetValue(DeliveryProperty, value);
        }

        public ObservableCollection<PackageEvent> Events { get; set; }
        public ObservableCollection<BaseLocation> AvailableLocations { get; set; }
        //public ObservableCollection<Package> AvailablePackages { get; set; }
        public BaseLocation SelectedLocation { get; set; }

        private static void OnDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PackageLogControl control && e.NewValue is Delivery delivery)
            {
                control.Events = new ObservableCollection<PackageEvent>(delivery.Log);
                control.AvailableLocations = new ObservableCollection<BaseLocation>(
                    App.CurrentEmployee.Employee.Workplace != null
                        ? new[] { App.CurrentEmployee.Employee.Workplace }
                        : []);
                //control.AvailablePackages = new ObservableCollection<Package>(delivery.Packages); // need to implement but not now
                control.SelectedLocation = control.AvailableLocations.FirstOrDefault();
                control.DataContext = control;
            }
        }


        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedLocation == null || string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newEvent = new PackageEvent(SelectedLocation, DescriptionBox.Text, Delivery.Package);
            Delivery.Log.Add(newEvent);
            Events.Add(newEvent);
            DescriptionBox.Clear();
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PackageEvent ev)
            {
                if (MessageBox.Show("Видалити цю подію?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Delivery.Log.Remove(ev);
                    Events.Remove(ev);
                }
            }
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PackageEvent ev)
            {
                MessageBox.Show("Функціонал редагування ще не реалізований. (Можна додати простий діалог)", "Інформація");
                // Optional: Swap out DataTemplate for a form UI like inline edit
            }
        }
    }
}
