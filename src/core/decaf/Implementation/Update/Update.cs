using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using decaf.common;
using decaf.state;
using decaf.state.ValueSources.Update;

namespace decaf.Implementation
{
    internal class Update :
        UpdateBase,
        IUpdate,
        IUpdateTable,
        IUpdateSet,
        IUpdateSetFromQuery
    {
        private Update(
            IUpdateQueryContext context,
            IQueryContainerInternal query)
            : base(query, context)
        {
            this.Context = context;
            this.Query.SetContext(this.Context);
        }

        public static Update Create(
            IUpdateQueryContext context,
            IQueryContainer query)
            => new Update(context, query as IQueryContainerInternal);

        /// <inheritdoc/>
        public IUpdateSetFromQuery From(Action<ISelectWithAlias> query)
        {
            base.FromQuery(query);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Output(string column)
        {
            this.Context.Output(state.Output.Create(column, OutputSources.Updated));
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Set<T>(string column, T value)
        {
            var col = Column.Create(column, this.Context.Table);
            var valueSource = StaticValueSource.Create(col, value);
            this.Context.Set(valueSource);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Set(dynamic values)
        {
            base.SetValues(new[] { values });
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSetFromQuery Set(string columnToUpdate, string sourceColumn)
        {
            var destination = Column.Create(columnToUpdate, this.Context.Table);
            var source = Column.Create(sourceColumn, this.Context.Source);
            this.Context.Set(QueryValueSource.Create(destination, source, this.Context.Source));
            return this;
        }

        /// <inheritdoc/>
        public IUpdateTable Table(string table, string alias = null)
        {
            var target = state.QueryTargets.TableTarget.Create(table, alias);
            this.Context.Update(target);
            return this;
        }

        /// <inheritdoc/>
        public IUpdateTable<T> Table<T>()
        {
            var table = this.Context.Helpers().GetTableName<T>();
            var alias = this.Query.AliasManager.Add(null, table);
            this.Context.Update(state.QueryTargets.TableTarget.Create(table, alias));
            return UpdateTyped<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc/>
        public IUpdateTable<T> Table<T>(Expression<Func<T, object>> expression)
        {
            var table = this.Context.Helpers().GetTableName(expression);
            var alias = this.Context.Helpers().GetTableAlias(expression);

            var managedTable = this.Query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.Query.AliasManager.Add(alias, table);
            this.Context.Update(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));

            return UpdateTyped<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc/>
        public IUpdateTable<T> Table<T>(string alias)
        {
            var table = this.Context.Helpers().GetTableName<T>();
            var managedTable = this.Query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.Query.AliasManager.Add(alias, table);
            this.Context.Update(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));

            return UpdateTyped<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc/>
        public IUpdateSet Where(Action<IWhereBuilder> builder)
        {
            var whereBuilder = WhereBuilder.Create(base.Query.Options, this.Context) as IWhereBuilderInternal;
            builder(whereBuilder);
            this.Context.Where(whereBuilder.GetClauses().First());
            return this;
        }

        /// <inheritdoc/>
        public IUpdateSet Where(IWhere clause)
        {
            if (clause == null) throw new ArgumentNullException(nameof(clause));
            this.Context.Where(clause);
            return this;
        }

        /// <inheritdoc/>
        IUpdateSetFromQuery IUpdateSetFromQuery.Output(string column)
        {
            this.Output(column);
            return this;
        }

        /// <inheritdoc/>
        IUpdateSetFromQuery IUpdateSetFromQuery.Where(Action<IWhereBuilder> builder)
        {
            this.Where(builder);
            return this;
        }
    }
}

