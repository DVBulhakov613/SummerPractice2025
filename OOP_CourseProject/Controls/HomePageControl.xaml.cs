using Class_Lib.Database.Repositories;
using Class_Lib;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls
{
    public partial class HomePageControl : UserControl
    {
        public HomePageControl()
        {
            InitializeComponent();
        }

        //private async void OpenQueryBuilder_Click(object sender, RoutedEventArgs e)
        //{
        //    var queryBuilder = new QueryBuilder(typeof(Employee), async conditions =>
        //    {
        //        // Combine expressions into one
        //        Expression<Func<Employee, bool>> finalPredicate = conditions.Aggregate((current, next) =>
        //            System.Linq.Expressions.Expression.Lambda<Func<Employee, bool>>(
        //                System.Linq.Expressions.Expression.AndAlso(current.Body, System.Linq.Expressions.Expression.Invoke(next, current.Parameters)),
        //                current.Parameters));

        //        // Query repository
        //        var repo = new EmployeeRepository(); // or however you access it
        //        var results = await repo.GetByCriteriaAsync(finalPredicate);

        //        // Now show results in your main window
        //        EmployeeResultsDataGrid.ItemsSource = results;
        //    });

        //    queryBuilder.ShowDialog(); // or Show()
        //}

    }
}
