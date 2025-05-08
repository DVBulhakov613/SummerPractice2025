using Class_Lib.Backend.Database;
using GalaSoft.MvvmLight.Command;
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
using OOP_CourseProject.Controls.ViewModel;

namespace OOP_CourseProject.Controls
{
    /// <summary>
    /// Interaction logic for UniversalDataControl.xaml
    /// </summary>
    public partial class UniversalDataControl : UserControl
    {
        public ICommand ItemClickCommand { get; }

        public UniversalDataControl()
        {
            InitializeComponent();
            ItemClickCommand = new RelayCommand<InfoItem>(param =>
            {
                if (param is InfoItem item && item.OnClick != null)
                {
                    item.OnClick.Invoke();
                }
            });

            DataContext = this;
        }
    }
}
