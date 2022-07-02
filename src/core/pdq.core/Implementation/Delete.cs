using System;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete : Execute, IDelete, IDeleteFrom
	{
        private readonly IDeleteQueryContext context;

        private Delete(IQuery query) : base((IQueryInternal)query)
        {
            this.context = DeleteQueryContext.Create(this.query.AliasManager);
            this.query.SetContext(this.context);
        }

        public static Delete Create(IQuery query) => new Delete(query);

        /// <inheritdoc />
        public void Dispose() => this.context.Dispose();

        /// <inheritdoc />
        public IDeleteFrom From(string name, string alias = null, string schema = null)
        {
            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? name;
            var managedAlias = this.query.AliasManager.Add(name, alias);
            context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias, schema));
            return this;
        }

        /// <inheritdoc />
        public IDeleteFrom<T> From<T>(Expression<Func<T, T>> expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            var alias = this.context.Helpers().GetTableAlias(expression);

            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(table, alias);

            this.context.From(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return Delete<T>.Create(this.context, this.query);
        }

        /// <inheritdoc />
        public IDeleteFrom<T> From<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            var alias = this.query.AliasManager.Add(table, null);
            this.context.From(state.QueryTargets.TableTarget.Create(table, alias));
            return Delete<T>.Create(this.context, this.query);
        }

        /// <inheritdoc />
        public IDeleteFrom Where(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.context);
            builder(b);
            context.Where(b.GetClauses().First());
            return this;
        }
    }
}

