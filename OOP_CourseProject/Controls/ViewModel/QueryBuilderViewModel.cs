using Class_Lib.Backend.Database;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace OOP_CourseProject.Controls
{
    public class QueryBuilderViewModel<T> : BaseViewModel where T : class
    {
        // Available fields and operators for query building
        public ObservableCollection<string> Fields { get; set; } = new();
        public ObservableCollection<string> Operators { get; set; } = new() { "==", "StartsWith", "Contains" };

        // User inputs
        public string SelectedField { get; set; }
        public string SelectedOperator { get; set; }
        public string ValueInput { get; set; }

        // List of raw conditions (data only)
        public ObservableCollection<QueryCondition> Conditions { get; set; } = new();

        // Query submission event
        public event Action<List<Expression<Func<T, bool>>>> QuerySubmitted;

        public QueryBuilderViewModel()
        {
            // Populate fields dynamically based on type T
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                Fields.Add(prop.Name);
            }
        }

        // Commands
        public ICommand AddConditionCommand => new RelayCommand(AddCondition);
        public ICommand SubmitQueryCommand => new RelayCommand(SubmitQuery);

        private void AddCondition()
        {
            if (!string.IsNullOrWhiteSpace(SelectedField)
                && !string.IsNullOrWhiteSpace(SelectedOperator)
                && !string.IsNullOrWhiteSpace(ValueInput))
            {
                var condition = new QueryCondition { Field = SelectedField, Operator = SelectedOperator, Value = ValueInput };
                Conditions.Add(condition);
            }
        }

        private void SubmitQuery()
        {
            var expressions = Conditions
                .Select(c => QueryBuilderService<T>.BuildFilter<T>(c.Field, c.Operator, c.Value))
                .ToList();

            QuerySubmitted?.Invoke(expressions);
        }
    }
}
