using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using decaf.common;
using decaf.state;

namespace decaf.Implementation
{
    internal class Insert :
        InsertBase,
        IInsert,
        IInsertInto,
        IInsertValues
    {
        private Insert(IQueryContainerInternal query, IInsertQueryContext context)
            : base(query, context) { }

        public static Insert Create(IInsertQueryContext context, IQueryContainer query)
            => new Insert(query as IQueryContainerInternal, context);

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
            var managedTable = this.Query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.Query.AliasManager.Add(alias, managedTable);
            this.Context.Into(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return this;
        }

        /// <inheritdoc/>
        public IInsertInto<T> Into<T>()
        {
            var table = this.Context.Helpers().GetTableName<T>();
            var alias = this.Query.AliasManager.Add(null, table);
            this.Context.Into(state.QueryTargets.TableTarget.Create(table, alias));

            return Insert<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc/>
        public IInsertInto<T> Into<T>(Expression<Func<T, object>> expression)
        {
            var table = this.Context.Helpers().GetTableName(expression);
            var alias = this.Context.Helpers().GetTableAlias(expression);

            var managedTable = this.Query.AliasManager.GetAssociation(alias) ?? table;
            var managedAlias = this.Query.AliasManager.Add(alias, table);

            this.Context.Into(state.QueryTargets.TableTarget.Create(managedTable, managedAlias));
            return Insert<T>.Create(this.Context, this.Query);
        }

        /// <inheritdoc/>
        public IInsertValues Output(string column)
        {
            var col = state.Column.Create(column, this.Context.Target);
            this.Context.Output(state.Output.Create(col, OutputSources.Inserted));
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

