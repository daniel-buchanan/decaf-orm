using System;
using System.Linq;
using System.Linq.Expressions;
using decaf.common;
using decaf.state;

namespace decaf.Implementation
{
	internal class Delete :
        Execute<IDeleteQueryContext>,
        IDelete,
        IDeleteFrom
	{
        private Delete(
            IDeleteQueryContext context,
            IQueryContainerInternal query)
            : base(query, context)
        {
            this.Context = context;
            this.Query.SetContext(this.Context);
        }

        public static Delete Create(
            IDeleteQueryContext context,
            IQueryContainer query)
            => new Delete(context, query as IQueryContainerInternal);

        /// <inheritdoc />
        public IDeleteFrom From(string name, string alias = null, string schema = null)
        {
            var managedTable = this.Query.AliasManager.GetAssociation(alias) ?? name;
            var managedAlias = this.Query.AliasManager.Add(alias, name);
            Context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias, schema));
            return this;
        }

        /// <inheritdoc />
        public IDeleteFrom<T> From<T>(Expression<Func<T, T>> aliasExpression)
        {
            var table = this.Context.Helpers().GetTableName(aliasExpression);
            var alias = this.Context.Helpers().GetTableAlias(aliasExpression);

            var managedTable = this.Query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.Query.AliasManager.Add(alias, table);

            this.Context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return Delete<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc />
        public IDeleteFrom<T> From<T>()
        {
            var table = this.Context.Helpers().GetTableName<T>();
            var alias = this.Query.AliasManager.Add(null, table);
            this.Context.From(state.QueryTargets.TableTarget.Create(table, alias));
            return Delete<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc />
        public IDeleteFrom Output(string column)
        {
            var col = state.Column.Create(column, this.Context.Table);
            this.Context.Output(state.Output.Create(col, OutputSources.Deleted));
            return this;
        }

        /// <inheritdoc />
        public IDeleteFrom Where(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.Query.Options, this.Context) as IWhereBuilderInternal;
            builder(b);
            var clause = b.GetClauses().First();
            if ((clause is state.Conditionals.And ||
               clause is state.Conditionals.Or) &&
               clause.Children.Count == 1)
            {
                clause = clause.Children.First();
            }

            this.Context.Where(clause);
            return this;
        }
    }
}

