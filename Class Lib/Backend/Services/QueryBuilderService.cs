using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Services
{
    public class QueryBuilderService
    {
        public List<Expression<Func<T, bool>>> BuildConditions<T>(List<QueryCondition> conditions)
        {
            var expressions = new List<Expression<Func<T, bool>>>();

            foreach (var condition in conditions)
            {
                var expression = BuildFilter<T>(condition.Field, condition.Operator, condition.Value);
                expressions.Add(expression);
            }

            return expressions;
        }

        private Expression<Func<T, bool>> BuildFilter<T>(string propertyName, string operation, object value)
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

    public class QueryCondition
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
    }
}
