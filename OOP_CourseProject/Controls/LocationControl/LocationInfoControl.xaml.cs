using Class_Lib;
using OOP_CourseProject.Controls.EmployeeControl.EmployeeControls;
using OOP_CourseProject.Controls.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_CourseProject.Controls.LocationControl
{
    /// <summary>
    /// Interaction logic for LocationInfoControl.xaml
    /// </summary>
    public partial class LocationInfoControl : UserControl
    {
        public LocationInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register(nameof(Location), typeof(BaseLocation), typeof(LocationInfoControl), new PropertyMetadata(null, OnDeliveryChanged));

        public BaseLocation Location
        {
            get => (BaseLocation)GetValue(LocationProperty);
            set => SetValue(LocationProperty, value);
        }

        public IInfoProviderViewModel LocationViewModel { get; private set; }

        private static void OnDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            if (d is LocationInfoControl control && e.NewValue is BaseLocation)
            {
                switch (e.NewValue)
                {
                    case PostalOffice postalOffice:
                        control.LocationViewModel = new PostalOfficeViewModel(postalOffice);
                        break;
                    case Warehouse warehouse:
                        control.LocationViewModel = new WarehouseViewModel(warehouse);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
