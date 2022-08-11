using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;
using pdq.state.ValueSources.Update;

namespace pdq.Implementation
{
    internal class Update :
        UpdateBase,
        IUpdate,
        IUpdateTable,
        IUpdateSet
    {
        private Update(
            IUpdateQueryContext context,
            IQueryInternal query)
            : base(query, context)
        {
            this.context = context;
            this.query.SetContext(this.context);
        }

        public static Update Create(
            IUpdateQueryContext context,
            IQueryInternal query)
            => new Update(context, query);

        /// <inheritdoc/>
        public IUpdateSet From(Action<ISelectWithAlias> query)
        {
            base.FromQuery(query);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Output(string column)
        {
            this.context.AddOutput(state.Output.Create(column, OutputSources.Updated));
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Set<T>(string column, T value)
        {
            var col = Column.Create(column, this.context.Table);
            var valueSource = StaticValueSource.Create(col, value);
            this.context.Set(valueSource);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Set(dynamic values)
        {
            base.SetValues(new[] { values });
            return this;
        }

        /// <inheritdoc/>
        public IUpdateTable Table(string table, string alias = null)
        {
            var target = state.QueryTargets.TableTarget.Create(table, alias);
            this.context.Update(target);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateTable<T> Table<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            var alias = this.query.AliasManager.Add(null, table);
            this.context.Update(state.QueryTargets.TableTarget.Create(table, alias));
            return UpdateTyped<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IUpdateTable<T> Table<T>(Expression<Func<T, object>> expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            var alias = this.context.Helpers().GetTableAlias(expression);

            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(alias, table);
            this.context.Update(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));

            return UpdateTyped<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IUpdateSet Where(Action<IWhereBuilder> builder)
        {
            var whereBuilder = WhereBuilder.Create(base.query.Options, this.context) as IWhereBuilderInternal;
            builder(whereBuilder);
            this.context.Where(whereBuilder.GetClauses().First());
            return this;
        }
    }
}

