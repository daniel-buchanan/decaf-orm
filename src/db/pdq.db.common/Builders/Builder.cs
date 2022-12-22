using System;
using System.Collections.Generic;
using pdq.common;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class Builder<T> : IBuilder<T>
		where T: IQueryContext
	{
		protected readonly IWhereBuilder whereBuilder;
		protected readonly IHashProvider hashProvider;
		protected readonly PdqOptions pdqOptions;

		protected Builder(
			IWhereBuilder whereBuilder,
			IHashProvider hashProvider,
			PdqOptions pdqOptions)
		{
			this.whereBuilder = whereBuilder;
			this.hashProvider = hashProvider;
			this.pdqOptions = pdqOptions;
		}

        protected abstract string CommentCharacter { get; }

        public SqlTemplate Build(T context)
		{
            var sqlBuilder = SqlBuilder.Create();
			var parameterManager = ParameterManager.Create(this.hashProvider);

			if(this.pdqOptions.IncludeHeaderCommentsInSql)
			{
                sqlBuilder.AppendLine("{0} pdq :: query hash: {1}", CommentCharacter, context.GetHash());
                sqlBuilder.AppendLine("{0} pdq :: generated at: {1}", CommentCharacter, DateTime.Now.ToString());
            }

			Build(context, sqlBuilder, parameterManager);

			return SqlTemplate.Create(sqlBuilder.GetSql(), parameterManager.GetParameters());
        }

        public Dictionary<string, object> GetParameters(T context)
        {
            var sqlBuilder = SqlBuilder.CreateNoOp();
            var parameterManager = ParameterManager.Create(this.hashProvider);
			GetParameters(context, sqlBuilder, parameterManager);

            return parameterManager.GetParameterValues();
        }

		protected abstract void GetParameters(T context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

		protected abstract void Build(T context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

    }
}

