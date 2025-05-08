using Class_Lib;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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

        private void InfoItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb && tb.DataContext is InfoItem item && item.OnClick != null)
            {
                item.OnClick();
            }
        }
    }
}
