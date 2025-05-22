using Class_Lib.Backend.Package_related;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls.ReceivePackageControls
{
    /// <summary>
    /// Interaction logic for PackageInfoControl.xaml
    /// </summary>
    public partial class ReceivePackageInfoControl : UserControl
    {
        public ReceivePackageInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PackageProperty =
            DependencyProperty.Register(nameof(Package), typeof(Delivery), typeof(ReceivePackageInfoControl), new PropertyMetadata(null, OnDeliveryChanged));

        public Delivery Package
        {
            get => (Delivery)GetValue(PackageProperty);
            set => SetValue(PackageProperty, value);
        }

        public IInfoProviderViewModel DeliveryViewModel { get; private set; }

        private static void OnDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ReceivePackageInfoControl control && e.NewValue is Delivery newPackage)
            {
                control.DeliveryViewModel = new DeliveryViewModel(newPackage);
            }
        }
    }
}
