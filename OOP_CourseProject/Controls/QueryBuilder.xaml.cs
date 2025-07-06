using Class_Lib;
using System;
using System.Linq.Expressions;
using System.Windows;

namespace OOP_CourseProject.Controls
{
    public partial class QueryBuilder : Window
    {
        //private readonly object _viewModel;
        //private readonly Type _viewModelType;
        //private readonly Delegate _onQuerySubmitted;

        //public QueryBuilder(Type entityType, Delegate onQuerySubmitted)
        //{
        //    //if (entityType == null)
        //    //{
        //    //    throw new ArgumentNullException(nameof(entityType), "Запит повинен мати тип об'єкту (Працівник, Вміст, Посилка ...).");
        //    //}
        //    //if (onQuerySubmitted == null)
        //    //{
        //    //    throw new ArgumentNullException(nameof(onQuerySubmitted), "Запит не може бути пустим.");
        //    //}

        //    //InitializeComponent();

        //    //_viewModelType = typeof(QueryBuilderViewModel<>).MakeGenericType(entityType);
        //    //_viewModel = Activator.CreateInstance(_viewModelType);

        //    //// Subscribe to the event
        //    //var eventInfo = _viewModelType.GetEvent("QuerySubmitted");
        //    //eventInfo.AddEventHandler(_viewModel, onQuerySubmitted);

        //    //DataContext = _viewModel;

        //    //Closed += (s, e) => {
        //    //    eventInfo.RemoveEventHandler(_viewModel, onQuerySubmitted);
        //    //};
        //}

        // commit comment (for a dummy commit)

        public QueryBuilder()
        {

        }
    }
}
