using System;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.Implementation
{
    internal class SelectTypedBase : SelectBase
	{
        protected SelectTypedBase(
            ISelectQueryContext context,
            IQueryInternal query)
            : base(context, query)
        {
        }

        protected void AddFrom<T>(Expression<Func<T, object>> expression = null)
        {
            string managedTable, managedAlias;

            if (expression is null)
            {
                managedTable = this.context.Helpers().GetTableName<T>();
                managedAlias = this.query.AliasManager.Add(null, managedTable);
            }
            else
            {
                var table = this.context.Helpers().GetTableName(expression);
                var alias = this.context.Helpers().GetTableAlias(expression);
                managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
                managedAlias = this.query.AliasManager.Add(alias, table);
            }

            this.context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
        }

        protected void AddJoin<T1, T2>(Expression expression, JoinType type)
        {
            var conditions = this.context.Helpers().ParseJoin(expression);
            if (conditions == null)
                conditions = this.context.Helpers().ParseWhere(expression);
            var left = GetQueryTarget<T1>();
            var right = GetQueryTarget<T2>();

            this.context.Join(state.Join.Create(left, right, type, conditions));
        }

        protected void AddJoin<T1, T2>(Action<ISelectWithAlias> query, Expression expression, JoinType type)
        {
            var select = Select.Create(this.context, this.query);
            query(select);

            var table = this.context.Helpers().GetTableName<T2>();
            var alias = this.query.AliasManager.Add(select.Alias, table);

            var left = GetQueryTarget<T1>();
            var right = SelectQueryTarget.Create(select.GetContext(), alias);

            var conditions = this.context.Helpers().ParseJoin(expression);
            if (conditions == null)
                conditions = this.context.Helpers().ParseWhere(expression);

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
            var target = GetQueryTargetFromExpression(expression);
            this.context.GroupBy(state.GroupBy.Create(column, target));
        }

        protected void AddOrderBy(Expression expression, SortOrder order)
        {
            var column = this.context.Helpers().GetColumnName(expression);
            var target = GetQueryTargetFromExpression(expression);
            this.context.OrderBy(state.OrderBy.Create(column, target, order));
        }

        private IQueryTarget GetQueryTargetFromExpression(Expression expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            return GetQueryTargetByTable(table);
        }

        protected IQueryTarget GetQueryTarget<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            return GetQueryTargetByTable(table);
        }
    }
}

