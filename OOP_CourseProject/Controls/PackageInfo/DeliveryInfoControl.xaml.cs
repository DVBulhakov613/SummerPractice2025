using Class_Lib;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Package_related;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls.PackageInfo
{
    /// <summary>
    /// Interaction logic for PackageInfoControl.xaml
    /// </summary>
    public partial class DeliveryInfoControl : UserControl
    {
        public DeliveryInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DeliveryProperty =
            DependencyProperty.Register(nameof(Delivery), typeof(Delivery), typeof(DeliveryInfoControl), new PropertyMetadata(null, OnDeliveryChanged));

        public Delivery Delivery
        {
            get => (Delivery)GetValue(DeliveryProperty);
            set => SetValue(DeliveryProperty, value);
        }

        public IInfoProviderViewModel DeliveryViewModel { get; private set; }

        private static void OnDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DeliveryInfoControl control && e.NewValue is Delivery newDelivery)
            {
                control.DeliveryViewModel = new DeliveryViewModel(newDelivery);
                // control.DataContext = control.DeliveryViewModel;
            }
        }
    }


}
