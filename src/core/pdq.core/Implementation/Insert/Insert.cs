using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
    internal class Insert :
        InsertBase,
        IInsert,
        IInsertInto,
        IInsertValues
    {
        private Insert(IQueryContainerInternal query, IInsertQueryContext context)
            : base(query, context) { }

        public static Insert Create(IInsertQueryContext context, IQueryContainerInternal query)
            => new Insert(query, context);

        /// <inheritdoc/>
        public IInsertValues Columns(Expression<Func<IInsertColumnBuilder, dynamic>> columns)
        {
            base.AddColumns(columns);
            return this;
        }

        /// <inheritdoc/>
        public IInsertValues From(Action<ISelect> query)
        {
            base.FromQuery(query);
            return this;
        }

        /// <inheritdoc/>
        public IInsertInto Into(string table, string alias = null)
        {
            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(alias, managedTable);
            this.context.Into(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return this;
        }

        /// <inheritdoc/>
        public IInsertInto<T> Into<T>()
        {
            var table = this.context.Helpers().GetTableName<T>();
            var alias = this.query.AliasManager.Add(null, table);
            this.context.Into(state.QueryTargets.TableTarget.Create(table, alias));

            return Insert<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IInsertInto<T> Into<T>(Expression<Func<T, object>> expression)
        {
            var table = this.context.Helpers().GetTableName(expression);
            var alias = this.context.Helpers().GetTableAlias(expression);

            var managedTable = this.query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.query.AliasManager.Add(alias, table);

            this.context.Into(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return Insert<T>.Create(this.context, this.query);
        }

        /// <inheritdoc/>
        public IInsertValues Output(string column)
        {
            var col = state.Column.Create(column, this.context.Target);
            this.context.Output(state.Output.Create(col, OutputSources.Inserted));
            return this;
        }

        /// <inheritdoc/>
        public IInsertValues Value(dynamic value) => Values(new[] { value });

        /// <inheritdoc/>
        public IInsertValues Values(IEnumerable<dynamic> values)
        {
            base.AddValues(values);
            return this;
        }
    }
}

