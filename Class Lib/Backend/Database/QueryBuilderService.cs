using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database
{
    /// <summary>
    /// A generic query builder for constructing LINQ queries in a fluent manner.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryBuilderService<T> where T : class
    {
        private IQueryable<T> _query;
        private User? _user;
        private readonly List<Expression<Func<T, bool>>> _filters = new();
        private readonly List<QueryCondition> _conditions = new();


        public QueryBuilderService(User? user, IQueryable<T> source)
        {
            _query = source;
            _user = user;
        }

        public QueryBuilderService<T> AddCondition(QueryCondition condition)
        {
            _conditions.Add(condition);
            return this;
        }

        public QueryBuilderService<T> AddConditions(IEnumerable<QueryCondition> conditions)
        {
            _conditions.AddRange(conditions);
            return this;
        }

        public Expression<Func<T, bool>> GetPredicate()
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = BuildExpressionRecursive<T>(_conditions, param);
            return Expression.Lambda<Func<T, bool>>(body ?? Expression.Constant(true), param);
        }


        public static Expression<Func<T, bool>> BuildFilter<T>(string propertyPath, string operation, object value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = parameter;

            foreach (var member in propertyPath.Split('.'))
            {
                var propInfo = property.Type.GetProperty(member) ?? throw new ArgumentException($"'{member}' не знайдено в '{property.Type.Name}'.");
                property = Expression.Property(property, member);
            }

            var targetType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
            var convertedValue = Convert.ChangeType(value, targetType);
            var constant = Expression.Constant(convertedValue, property.Type);

            Expression comparison = operation switch
            {
                "==" => Expression.Equal(property, constant),
                "!=" => Expression.NotEqual(property, constant),
                ">" => Expression.GreaterThan(property, constant),
                "<" => Expression.LessThan(property, constant),
                ">=" => Expression.GreaterThanOrEqual(property, constant),
                "<=" => Expression.LessThanOrEqual(property, constant),
                "StartsWith" => Expression.Call(property, "StartsWith", null, constant),
                "EndsWith" => Expression.Call(property, "EndsWith", null, constant),
                "Contains" => Expression.Call(property, "Contains", null, constant),
                _ => throw new NotSupportedException($"Operation '{operation}' is not supported.")
            };

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        private static Expression? BuildExpressionRecursive<T>(IEnumerable<QueryCondition> conditions, ParameterExpression parameter)
        {
            Expression? combined = null;

            foreach (var condition in conditions)
            {
                Expression next;

                if (condition.IsGroup)
                {
                    var groupExpr = BuildExpressionRecursive<T>(condition.Conditions!, parameter);
                    if (groupExpr == null) continue;
                    next = groupExpr;
                }
                else
                {
                    var filter = BuildFilter<T>(condition.Field!, condition.Operator!, condition.Value!);
                    next = Expression.Invoke(filter, parameter);
                }

                if (combined == null)
                {
                    combined = next;
                }
                else
                {
                    combined = condition.Logic.ToUpper() switch
                    {
                        "AND" => Expression.AndAlso(combined, next),
                        "OR" => Expression.OrElse(combined, next),
                        _ => throw new NotSupportedException($"Логічний оператор '{condition.Logic}' не підтримується.")
                    };
                }
            }

            return combined;
        }

    }

    public class QueryCondition
    {
        public string? Field { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }

        public string Logic { get; set; } = "AND"; // "AND" or "OR"
        public List<QueryCondition>? Conditions { get; set; } // For grouping

        public bool IsGroup => Conditions != null && Conditions.Count > 0;
    }

}
