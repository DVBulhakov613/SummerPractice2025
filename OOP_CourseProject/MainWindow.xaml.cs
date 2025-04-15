using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Class_Lib;

namespace OOP_CourseProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        #region debug
        //private void TestDatabase_Click(object sender, RoutedEventArgs e)
        //{
        //    using (var context = new AppDbContext())
        //    {
        //        var countries = context.Countries.ToList();
        //        MessageBox.Show($"Number of countries: {countries.Count}");
        //    }
        //}


        #endregion
    }
}