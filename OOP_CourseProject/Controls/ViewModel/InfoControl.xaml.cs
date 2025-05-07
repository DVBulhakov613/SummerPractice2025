using Class_Lib;
using Class_Lib.Backend.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OOP_CourseProject.Controls.ViewModel
{
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty InfoProviderProperty = DependencyProperty.Register(
            nameof(InfoProvider),
            typeof(IInfoProviderViewModel),
            typeof(InfoControl),
            new PropertyMetadata(null, OnInfoProviderChanged));

        public IInfoProviderViewModel InfoProvider
        {
            get => (IInfoProviderViewModel)GetValue(InfoProviderProperty);
            set => SetValue(InfoProviderProperty, value);
        }

        private static void OnInfoProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (InfoControl)d;
            control.DataContext = e.NewValue;
        }
    }
}
