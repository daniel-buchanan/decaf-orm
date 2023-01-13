using System;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class DeleteBuilder : Builder<IDeleteQueryContext>
	{
        protected DeleteBuilder(
            IWhereBuilder whereBuilder,
            IHashProvider hashProvider,
            PdqOptions pdqOptions)
            : base(whereBuilder, hashProvider, pdqOptions)
        {
        }

        protected abstract void AddTables(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        protected abstract void AddOutput(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        private void AddWhere(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
            => this.whereBuilder.AddWhere(context.WhereClause, sqlBuilder, parameterManager);

        protected override void Build(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.AppendLine("{0} from", Constants.Delete);
            AddTables(context, sqlBuilder, parameterManager);
            AddWhere(context, sqlBuilder, parameterManager);
            AddOutput(context, sqlBuilder, parameterManager);
        }

        protected override void GetParameters(IDeleteQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            AddWhere(context, sqlBuilder, parameterManager);
        }
    }
}

