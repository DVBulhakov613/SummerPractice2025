using Microsoft.EntityFrameworkCore;
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
    public class QueryBuilder<T> where T : class
    {
        private IQueryable<T> _query;

        public QueryBuilder(IQueryable<T> source)
        {
            _query = source;
        }

        public QueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);
            return this;
        }

        public QueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _query = _query.OrderBy(keySelector);
            return this;
        }

        public QueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _query = _query.OrderByDescending(keySelector);
            return this;
        }

        public QueryBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationProperty)
        {
            _query = _query.Include(navigationProperty);
            return this;
        }

        public async Task<List<T>> ExecuteAsync()
        {
            return await _query.ToListAsync();
        }
    }

}
