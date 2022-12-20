using System;
using System.Collections.Generic;
using pdq.common;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class SelectBuilder : Builder<ISelectQueryContext>
	{
        public SelectBuilder(
            ISqlBuilder sqlBuilder,
            IWhereBuilder whereBuilder)
            : base(sqlBuilder, whereBuilder) { }

		protected abstract void AddColumns(ISelectQueryContext context);

		protected abstract void AddTables(ISelectQueryContext context, IList<string> parameters);

		protected abstract void AddJoins(ISelectQueryContext context, IList<string> parameters);

		protected abstract void AddOrderBy(ISelectQueryContext context);

		protected abstract void AddGroupBy(ISelectQueryContext context);

        public override SqlTemplate Build(ISelectQueryContext context)
        {
            var parameters = new List<string>();
            return Build(context, parameters);
        }

        protected SqlTemplate Build(ISelectQueryContext context, IList<string> parameters)
        {
            this.sqlBuilder.AppendLine("{0} pdq :: query hash: {1}", CommentCharacter, context.GetHash());
            this.sqlBuilder.AppendLine("{0} pdq :: generated at: {1}", CommentCharacter, DateTime.Now.ToString());
            this.sqlBuilder.AppendLine("select");
            AddColumns(context);
            AddTables(context, parameters);
            AddJoins(context, parameters);
            AddOrderBy(context);
            AddGroupBy(context);

            this.whereBuilder.AddWhere(context.WhereClause, this.sqlBuilder);

            return SqlTemplate.Create(this.sqlBuilder.GetSql(), parameters);
        }
    }
}

