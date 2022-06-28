using System;
using System.Linq;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete : Execute, IDelete, IDeleteFrom
	{
        private readonly IDeleteQueryContext context;

        private Delete(IQuery query) : base((IQueryInternal)query)
        {
            this.context = DeleteQueryContext.Create();
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
        public IDeleteFrom Where(Action<IWhereBuilder> builder)
        {
            var b = WhereBuilder.Create(this.context);
            builder(b);
            context.Where(b.GetClauses().First());
            return this;
        }
    }
}

