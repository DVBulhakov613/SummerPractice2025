using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
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
        private Employee? _user;
        private readonly List<Expression<Func<T, bool>>> _filters = new();

        private int? _skip;
        private int? _take;
        private Expression<Func<T, object>>? _orderBy;
        private bool _orderDescending;

        public QueryBuilderService(Employee? user, IQueryable<T> source)
        {
            _query = source;
            _user = user;
        }

        public void AddCondition(QueryCondition condition)
        {
            var filter = BuildFilter<T>(condition.Field, condition.Operator, condition.Value);
            _filters.Add(filter);
        }

        public void AddConditions(List<QueryCondition> conditions)
        {
            foreach (var c in conditions)
                _filters.Add(BuildFilter<T>(c.Field, c.Operator, c.Value));
        }

        public QueryBuilderService<T> Skip(int count)
        {
            _skip = count;
            return this;
        }

        public QueryBuilderService<T> Take(int count)
        {
            _take = count;
            return this;
        }

        public QueryBuilderService<T> OrderBy(Expression<Func<T, object>> keySelector)
        {
            _orderBy = keySelector;
            _orderDescending = false;
            return this;
        }

        public QueryBuilderService<T> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            _orderBy = keySelector;
            _orderDescending = true;
            return this;
        }

        /// <summary>
        /// Executes with the specified predicate. Ignores previously added conditions. Use with caution.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public QueryBuilderService<T> Where(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);
            return this;
        }

        public async Task<List<T>> ExecuteAsync()
        {
            var permission = AccessService.GetReadPermissionForType(typeof(T));
            if (_user != null && permission.HasValue && !AccessService.CanPerformAction(_user, (int)permission.Value))
            {
                throw new UnauthorizedAccessException($"Немає доступу до {typeof(T).Name}.");
            }

            foreach (var filter in _filters)
                _query = _query.Where(filter);

            if (_orderBy != null)
                _query = _orderDescending ? _query.OrderByDescending(_orderBy) : _query.OrderBy(_orderBy);

            if (_skip.HasValue)
                _query = _query.Skip(_skip.Value);

            if (_take.HasValue)
                _query = _query.Take(_take.Value);

            try
            {
                return await _query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Не вдалося виконати запит для типу {typeof(T).Name}", ex);
            }
        }

        public async Task<List<TResult>> ExecuteAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            var permission = AccessService.GetReadPermissionForType(typeof(T));

            if (permission.HasValue && !AccessService.CanPerformAction(_user, (int)permission.Value))
            {
                throw new UnauthorizedAccessException($"You do not have permission to read {typeof(T).Name} data.");
            }

            foreach (var filter in _filters)
                _query = _query.Where(filter);

            return await _query.Select(selector).ToListAsync();
        }


        //public List<Expression<Func<T, bool>>> BuildConditions<T>(List<QueryCondition> conditions)
        //{
        //    var expressions = new List<Expression<Func<T, bool>>>();

        //    foreach (var condition in conditions)
        //    {
        //        var expression = BuildFilter<T>(condition.Field, condition.Operator, condition.Value);
        //        expressions.Add(expression);
        //    }

        //    return expressions;
        //}

        public static Expression<Func<T, bool>> BuildFilter<T>(string propertyPath, string operation, object value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = parameter;

            foreach (var member in propertyPath.Split('.'))
            {
                var propInfo = property.Type.GetProperty(member);
                if (propInfo == null)
                    throw new ArgumentException($"'{member}' не знайдено в '{property.Type.Name}'.");

                property = Expression.Property(property, member);
            }

            var constant = Expression.Constant(Convert.ChangeType(value, property.Type));

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

    }

    public class QueryCondition
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }

        public QueryCondition(string field, string op, object value)
        {
            Field = field;
            Operator = op;
            Value = value;
        }
    }
}
