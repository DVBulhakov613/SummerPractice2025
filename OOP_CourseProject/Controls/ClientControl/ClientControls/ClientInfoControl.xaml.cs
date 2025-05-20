using Class_Lib.Backend.Person_related;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls.ClientControl.ClientControls
{
    /// <summary>
    /// Interaction logic for ClientInfoControl.xaml
    /// </summary>
    public partial class ClientInfoControl : UserControl
    {
        public ClientInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ClientProperty =
            DependencyProperty.Register(nameof(Client), typeof(Client), typeof(ClientInfoControl), new PropertyMetadata(null, OnDeliveryChanged));

        public Client Client
        {
            get => (Client)GetValue(ClientProperty);
            set => SetValue(ClientProperty, value);
        }

        public IInfoProviderViewModel ClientViewModel { get; private set; }

        private static void OnDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClientInfoControl control && e.NewValue is Client newClient)
            {
                control.ClientViewModel = new ClientViewModel(newClient);
            }
        }
    }
}
