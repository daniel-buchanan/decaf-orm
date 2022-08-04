using System;
using System.Linq;
using pdq.common;
using pdq.state;
using pdq.state.QueryTargets;

namespace pdq.Implementation
{
    internal class Join : IJoinFrom, IJoinTo, IJoinConditions
    {
        private readonly ISelectFrom selectFrom;
        private readonly IQueryContextInternal context;
        private readonly PdqOptions options;
        private readonly IQueryInternal query;
        private readonly JoinType joinType;

        private IQueryTarget left;
        private IQueryTarget right;

        private Join(
            ISelectFrom selectFrom,
            IQueryContext context,
            PdqOptions options,
            IQueryInternal query,
            JoinType joinType)
        {
            this.selectFrom = selectFrom;
            this.context = context as IQueryContextInternal;
            this.options = options;
            this.query = query;
            this.joinType = joinType;
        }

        public static Join Create(
            ISelectFrom selectFrom,
            IQueryContext context,
            PdqOptions options,
            IQueryInternal query,
            JoinType joinType) => new Join(selectFrom, context, options, query, joinType);

        /// <inheritdoc/>
        public IJoinTo From(string name, string alias, string schema = null)
        {
            var managedTable = this.context.AliasManager.GetAssociation(alias) ?? name;
            var managedAlias = this.context.AliasManager.Add(alias, managedTable);
            var existingTarget = this.context.QueryTargets.FirstOrDefault(t => t.Alias == managedAlias);
            if(existingTarget == null)
            {
                existingTarget = TableTarget.Create(managedTable, managedAlias, schema);
                this.context.AddQueryTarget(existingTarget);
            }
            this.left = existingTarget;
            return this;
        }

        /// <inheritdoc/>
        public ISelectFrom On(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.options, this.context) as IWhereBuilderInternal;
            builder(b);

            var selectContext = this.context as ISelectQueryContext;
            var conditions = b.GetClauses().First();
            var join = state.Join.Create(this.left, this.right, this.joinType, conditions);
            selectContext.Join(join);

            return this.selectFrom;
        }

        /// <inheritdoc/>
        public IJoinConditions To(string name, string alias, string schema = null)
        {
            var managedTable = this.context.AliasManager.GetAssociation(alias) ?? name;
            var managedAlias = this.context.AliasManager.Add(alias, managedTable);
            var existingTarget = this.context.QueryTargets.FirstOrDefault(t => t.Alias == managedAlias);
            if (existingTarget == null)
            {
                existingTarget = TableTarget.Create(managedTable, managedAlias, schema);
                this.context.AddQueryTarget(existingTarget);
            }
            this.right = existingTarget;
            return this;
        }

        /// <inheritdoc/>
        public IJoinConditions To(Action<ISelectWithAlias> query)
        {
            var selectContext = SelectQueryContext.Create(this.context.AliasManager);
            var select = Select.Create(selectContext, this.query);
            query(select);
            
            this.right = SelectQueryTarget.Create(selectContext, select.Alias);
            return this;
        }
    }
}

