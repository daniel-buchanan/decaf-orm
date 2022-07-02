using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.Exceptions;
using pdq.state;

namespace pdq.Implementation
{
	internal class Select<T>
        : Execute,
        ISelectFromTyped<T>,
        ISelectColumnTyped<T>,
        IGroupByTyped<T>,
        IGroupByThenTyped<T>,
        IOrderByThenTyped<T>
	{
        private readonly ISelectQueryContext context;

        private Select(
            ISelectQueryContext context,
            IQuery query)
            : base((IQueryInternal)query)
        {
            this.context = context;
        }

        public static Select<T> Create(
            ISelectQueryContext context,
            IQuery query)
            => new Select<T>(context, query);

        private IQueryTarget GetTarget(string alias)
        {
            return this.context.QueryTargets.FirstOrDefault(t => t.Alias == alias);
        }

        public ISelectFromTyped<T> Column(Expression<Func<T, object>> expression)
        {
            var table = this.context.Helpers().GetTableName<T>();
            var managedAlias = this.query.AliasManager.FindByAssociation(table).FirstOrDefault()?.Name;

            if (string.IsNullOrWhiteSpace(managedAlias))
            {
                var tableName = this.context.Helpers().GetTableName<T>();
                var alias = this.context.Helpers().GetTableAlias(expression);
                throw new TableNotFoundException(alias, tableName);
            }

            var column = this.context.Helpers().GetColumnName(expression);

            this.context.Select(state.Column.Create(column, GetTarget(managedAlias)));
            return this;
        }

        public ISelectFromTyped<T> Columns(Expression<Func<T, dynamic>> selectExpression)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => this.context.Dispose();

        public IGroupByThenTyped<T> GroupBy(Expression<Func<T, object>> builder)
        {
            throw new NotImplementedException();
        }

        public ISelectFromTyped<T, TDestination> Join<TDestination>(Expression<Func<T, TDestination, bool>> joinExpression, JoinType type = JoinType.Default)
        {
            
            throw new NotImplementedException();
        }

        public IOrderByThenTyped<T> OrderBy(Expression<Func<T, object>> builder)
        {
            throw new NotImplementedException();
        }

        public IOrderByThenTyped<T> ThenBy(Expression<Func<T, object>> builder)
        {
            throw new NotImplementedException();
        }

        public IGroupByTyped<T> Where(Expression<Func<T, bool>> builder)
        {
            throw new NotImplementedException();
        }

        IGroupByThenTyped<T> IGroupByThenTyped<T>.ThenBy(Expression<Func<T, object>> builder)
        {
            throw new NotImplementedException();
        }
    }
}

