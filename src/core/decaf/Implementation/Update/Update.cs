﻿using System;
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
            this.context = context;
            this.query.SetContext(this.context);
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
            this.context.Output(state.Output.Create(column, OutputSources.Updated));
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
        public IUpdateSetFromQuery Set(string columnToUpdate, string sourceColumn)
        {
            var destination = Column.Create(columnToUpdate, this.context.Table);
            var source = Column.Create(sourceColumn, this.context.Source);
            this.context.Set(QueryValueSource.Create(destination, source, this.context.Source));
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
        public IUpdateTable<T> Table<T>(string alias)
        {
            var table = this.context.Helpers().GetTableName<T>();
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

        /// <inheritdoc/>
        public IUpdateSet Where(IWhere clause)
        {
            if (clause == null) throw new ArgumentNullException(nameof(clause));
            this.context.Where(clause);
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

