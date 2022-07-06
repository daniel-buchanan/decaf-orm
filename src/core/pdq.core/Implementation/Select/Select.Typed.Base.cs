using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.Exceptions;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.Implementation
{
	internal class SelectTypedBase : Execute
	{
        protected readonly ISelectQueryContext context;

        protected SelectTypedBase(
            ISelectQueryContext context,
            IQuery query)
            : base((IQueryInternal)query)
        {
            this.context = context;
        }

        protected void AddColumns(Expression expression)
        {
            var properties = this.context.Helpers().GetPropertyInformation(expression);
            foreach(var p in properties)
            {
                var target = GetQueryTarget(p.Type);
                this.context.Select(state.Column.Create(p.Name, target, p.NewName));
            }
        }

        private IQueryTarget GetQueryTarget<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            return GetQueryTarget(table);
        }

        private IQueryTarget GetQueryTarget(Type type)
        {
            var table = this.context.Helpers().GetTableName(type);
            return GetQueryTarget(table);
        }

        private IQueryTarget GetQueryTarget(Expression expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            return GetQueryTarget(table);
        }

        private IQueryTarget GetQueryTarget(string table)
        {
            var alias = this.query.AliasManager.FindByAssociation(table).FirstOrDefault();
            if (alias == null) throw new TableNotFoundException(alias.Name, table);

            var target = this.context.QueryTargets.FirstOrDefault(t => t.Alias == alias.Name);
            if (target == null) throw new TableNotFoundException(alias.Name, table);
            return target;
        }

        protected void AddJoin<T1, T2>(Expression expression, JoinType type)
        {
            var conditions = this.context.Helpers().ParseWhere(expression);
            var left = GetQueryTarget<T1>();
            var right = GetQueryTarget<T2>();

            this.context.Join(state.Join.Create(left, right, type, conditions));
        }

        protected void AddJoin<T1, T2>(Action<ISelectWithAlias> query, Expression expression, JoinType type)
        {
            var select = Select.Create(this.query);
            query(select);

            var table = this.context.Helpers().GetTableName<T2>();
            var alias = this.query.AliasManager.Add(table, select.Alias);

            var left = GetQueryTarget<T1>();
            var right = SelectQueryTarget.Create(select.GetContext(), alias);

            var conditions = this.context.Helpers().ParseWhere(expression);

            this.context.Join(state.Join.Create(left, right, type, conditions));
        }

        protected void AddWhere(Expression expression)
        {
            var clause = this.context.Helpers().ParseWhere(expression);
            this.context.Where(clause);
        }

        protected void AddGroupBy(Expression expression)
        {
            var column = this.context.Helpers().GetColumnName(expression);
            var target = GetQueryTarget(expression);
            this.context.GroupBy(state.GroupBy.Create(column, target));
        }

        protected void AddOrderBy(Expression expression, SortOrder order)
        {
            var column = this.context.Helpers().GetColumnName(expression);
            var target = GetQueryTarget(expression);
            this.context.OrderBy(state.OrderBy.Create(column, target, order));
        }
    }
}

