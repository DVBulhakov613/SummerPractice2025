using Class_Lib;
using Class_Lib.Backend.Person_related;
using OOP_CourseProject.Controls;
using System.Linq.Expressions;
using System.Windows;

namespace OOP_CourseProject
{
    public partial class MainWindow : Window
    {
        //// dictionary of query methods for different entity types, used for dynamic query building
        //private readonly Dictionary<Type, Func<Expression<Func<object, bool>>, Task<IEnumerable<object>>>> _queryMethods =
        //    new Dictionary<Type, Func<Expression<Func<object, bool>>, Task<IEnumerable<object>>>>
        //    {
        //        { typeof(Employee), condition => PersonMethodsInstance.GetEmployeesByCriteriaAsync(condition) },
        //        { typeof(Client), condition => ClientMethodsInstance.GetClientsByCriteriaAsync(condition) },
        //        { typeof(Package), condition => PackageMethodsInstance.GetPackagesByCriteriaAsync(condition) },
        //        { typeof(PackageEvent), condition => LocationMethodsInstance.GetLocationsByCriteriaAsync(condition) },
        //        { typeof(Content), condition => LocationMethodsInstance.GetLocationsByCriteriaAsync(condition) },
        //        { typeof(User), condition => LocationMethodsInstance.GetLocationsByCriteriaAsync(condition) },
        //    };

        public MainWindow()
        {
            InitializeComponent();

            // Load HomePage by default
            MainContent.Content = new HomePageControl();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomePageControl();
        }

        private void SendPackageButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SendPackageControl();
        }

        //private void OpenQueryBuilder_Click(object sender, RoutedEventArgs e)
        //{
        //    // Example: Open query builder for Employee
        //    var entityType = typeof(Employee); // Change this dynamically based on the table
        //    var queryMethod = _queryMethods[entityType];

        //    var queryBuilderWindow = new QueryBuilder(
        //        entityType,
        //        conditions => OnQuerySubmitted(entityType, queryMethod, conditions)
        //    );
        //    queryBuilderWindow.Show();
        //}

        //private async void OnQuerySubmitted(Type entityType, Func<Expression<Func<object, bool>>, Task<IEnumerable<object>>> queryMethod, List<LambdaExpression> conditions)
        //{
        //    // Combine all conditions into one
        //    var combinedCondition = conditions.Aggregate((current, next) => current.And(next));

        //    // Execute the query using the specified method
        //    IEnumerable<object> results;
        //    using (var context = App.DbContextFactory.CreateDbContext())
        //    {
        //        results = await queryMethod(combinedCondition);
        //    }

        //    // Update the appropriate table in the parent window
        //    if (entityType == typeof(Employee))
        //    {
        //        EmployeeTable.ItemsSource = results.Cast<Employee>();
        //    }
        //    else if (entityType == typeof(Client))
        //    {
        //        ClientTable.ItemsSource = results.Cast<Client>();
        //    }
        //    else if (entityType == typeof(Package))
        //    {
        //        PackageTable.ItemsSource = results.Cast<Package>();
        //    }
        //    else if (entityType == typeof(BaseLocation))
        //    {
        //        LocationTable.ItemsSource = results.Cast<BaseLocation>();
        //    }
        //}



    }
}
