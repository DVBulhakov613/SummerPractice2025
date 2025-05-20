using Class_Lib;
using Class_Lib.Backend.Person_related;
using OOP_CourseProject.Controls.ClientControl.ClientControls;
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

namespace OOP_CourseProject.Controls.EmployeeControl.EmployeeControls
{
    /// <summary>
    /// Interaction logic for EmployeeInfoControl.xaml
    /// </summary>
    public partial class EmployeeInfoControl : UserControl
    {
        public EmployeeInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EmployeeProperty =
            DependencyProperty.Register(nameof(Employee), typeof(Employee), typeof(EmployeeInfoControl), new PropertyMetadata(null, OnDeliveryChanged));

        public Employee Employee
        {
            get => (Employee)GetValue(EmployeeProperty);
            set => SetValue(EmployeeProperty, value);
        }

        public IInfoProviderViewModel EmployeeViewModel { get; private set; }

        private static void OnDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EmployeeInfoControl control && e.NewValue is Employee newEmployee)
            {
                control.EmployeeViewModel = new EmployeeViewModel(newEmployee);
            }
        }
    }
}
