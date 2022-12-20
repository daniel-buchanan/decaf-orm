using System;
using pdq.common;

namespace pdq.db.common.Builders
{
	public abstract class Builder<T> : IBuilder<T>
		where T: IQueryContext
	{
		protected readonly IWhereBuilder whereBuilder;
		protected readonly ISqlBuilder sqlBuilder;

		public Builder(ISqlBuilder sqlBuilder, IWhereBuilder whereBuilder)
		{
			this.sqlBuilder = sqlBuilder;
			this.whereBuilder = whereBuilder;
		}

		public abstract SqlTemplate Build(T context);

		protected abstract string CommentCharacter { get; }
    }
}

