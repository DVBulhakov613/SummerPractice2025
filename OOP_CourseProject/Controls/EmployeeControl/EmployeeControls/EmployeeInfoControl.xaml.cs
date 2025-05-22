using Class_Lib;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;

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
