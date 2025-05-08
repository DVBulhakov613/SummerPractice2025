using Class_Lib.Database.Repositories;
using Class_Lib;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using OOP_CourseProject.Controls.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Location_related.Methods;

namespace OOP_CourseProject.Controls
{
    public partial class HomePageControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public HomePageControl()
        {
            InitializeComponent();

            GenerateViewModel();
        }

        public async void GenerateViewModel()
        {
            var repo = App.AppHost.Services.GetRequiredService<LocationMethods>();

            DataContext = ViewModelService.CreateViewModel(await repo.GetByCriteriaAsync(App.CurrentEmployee, p => p.ID > 0));
        }

        //private IInfoProviderViewModel CreateViewModel(object model)
        //{
        //    return model switch
        //    {
        //        Package p => new PackageInfoViewModel(p),
        //        //Employee e => new EmployeeInfoViewModel(e),
        //        _ => throw new NotSupportedException($"Unsupported model type: {model.GetType().Name}")
        //    };
        //}


    }
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