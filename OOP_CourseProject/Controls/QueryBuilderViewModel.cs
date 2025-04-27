using Class_Lib;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

namespace OOP_CourseProject.Controls
{
    public class QueryBuilderViewModel : BaseViewModel
    {
        // fields for the query builder
        public ObservableCollection<string> Fields { get; set; } = new ObservableCollection<string> { "Name", "WorkplaceID", "Position" };
        public ObservableCollection<string> Operators { get; set; } = new ObservableCollection<string> { "==", "StartsWith", "Contains" };

        // user-selected values
        public string SelectedField { get; set; }
        public string SelectedOperator { get; set; }
        public string ValueInput { get; set; }

        // list of conditions
        public ObservableCollection<string> Conditions { get; set; } = new ObservableCollection<string>();

        // event to send the query back to the parent window
        public event Action<List<Expression<Func<Employee, bool>>>> QuerySubmitted;

        public QueryBuilderViewModel(Type entityType)
        {
            // populate fields dynamically based on the entity type
            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                Fields.Add(property.Name);
            }
        }

        // commands
        public ICommand AddConditionCommand => new RelayCommand(AddCondition);
        public ICommand SubmitQueryCommand => new RelayCommand(SubmitQuery);

        // internal list of conditions
        private readonly List<Expression<Func<Employee, bool>>> _conditionExpressions = new List<Expression<Func<Employee, bool>>>();

        // add a condition to the query
        private void AddCondition()
        {
            if (!string.IsNullOrEmpty(SelectedField) && !string.IsNullOrEmpty(SelectedOperator) && !string.IsNullOrEmpty(ValueInput))
            {
                // add a readable condition to the list
                Conditions.Add($"{SelectedField} {SelectedOperator} {ValueInput}");

                // build the LINQ expression
                var condition = BuildFilter<Employee>(SelectedField, SelectedOperator, ValueInput);
                _conditionExpressions.Add(condition);
            }
        }

        // submit the query back to the parent window
        private void SubmitQuery()
        {
            QuerySubmitted?.Invoke(_conditionExpressions);
        }

        // build a filter dynamically
        private static Expression<Func<T, bool>> BuildFilter<T>(string propertyName, string operation, object value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value);

            Expression comparison = operation switch
            {
                "==" => Expression.Equal(property, constant),
                "!=" => Expression.NotEqual(property, constant),
                ">" => Expression.GreaterThan(property, constant),
                "<" => Expression.LessThan(property, constant),
                "StartsWith" => Expression.Call(property, "StartsWith", null, constant),
                "Contains" => Expression.Call(property, "Contains", null, constant),
                _ => throw new NotSupportedException($"Operation '{operation}' is not supported.")
            };

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }
    }
}