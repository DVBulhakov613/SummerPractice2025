using Class_Lib;
using System;
using System.Linq.Expressions;
using System.Windows;

namespace OOP_CourseProject.Controls
{
    public partial class QueryBuilder : Window
    {
        private readonly QueryBuilderViewModel _viewModel;

        public QueryBuilder(Type entityType, Action<List<Expression<Func<Employee, bool>>>> onQuerySubmitted)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType), "Запит повинен мати тип об'єкту (Працівник, Вміст, Посилка ...).");
            }
            if (onQuerySubmitted == null)
            {
                throw new ArgumentNullException(nameof(onQuerySubmitted), "Запит не може бути пустим.");
            }

            InitializeComponent();

            // initialize the ViewModel
            _viewModel = new QueryBuilderViewModel(entityType);
            _viewModel.QuerySubmitted += onQuerySubmitted;

            // set the DataContext
            DataContext = _viewModel;

            // unsubscribe from the event when the window is closed (to avoid memory leaks)
            Closed += (s, e) => _viewModel.QuerySubmitted -= onQuerySubmitted;
        }
    }
}
