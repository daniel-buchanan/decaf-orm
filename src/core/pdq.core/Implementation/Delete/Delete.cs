using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete :
        Execute<IDeleteQueryContext>,
        IDelete,
        IDeleteFrom
	{
        private Delete(
            IDeleteQueryContext context,
            IQueryInternal query)
            : base(query, context, query.SqlFactory)
        {
            this.context = context;
            this.query.SetContext(this.context);
        }

        public static Delete Create(
            IDeleteQueryContext context,
            IQueryInternal query)
            => new Delete(context, query);

        /// <inheritdoc />
        public IDeleteFrom From(string name, string alias = null, string schema = null)
        {
            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? name;
            var managedAlias = this.query.AliasManager.Add(alias, name);
            context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias, schema));
            return this;
        }

        /// <inheritdoc />
        public IDeleteFrom<T> From<T>(Expression<Func<T, T>> aliasExpression)
        {
            var table = this.context.Helpers().GetTableName(aliasExpression);
            var alias = this.context.Helpers().GetTableAlias(aliasExpression);

            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? alias;
            var managedAlias = this.query.AliasManager.Add(alias, table);

            this.context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return Delete<T>.Create(this.context, this.query, this.sqlFactory);
        }

        /// <inheritdoc />
        public IDeleteFrom<T> From<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            var alias = this.query.AliasManager.Add(null, table);
            this.context.From(state.QueryTargets.TableTarget.Create(table, alias));
            return Delete<T>.Create(this.context, this.query, this.sqlFactory);
        }

        /// <inheritdoc />
        public IDeleteFrom Where(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.query.Options, this.context) as IWhereBuilderInternal;
            builder(b);
            context.Where(b.GetClauses().First());
            return this;
        }
    }
}

