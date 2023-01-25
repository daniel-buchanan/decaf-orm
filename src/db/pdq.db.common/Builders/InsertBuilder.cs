using System;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class InsertBuilder : Builder<IInsertQueryContext>
	{
        protected InsertBuilder(
            IWhereBuilder whereBuilder,
            IHashProvider hashProvider,
            PdqOptions pdqOptions)
            : base(whereBuilder, hashProvider, pdqOptions)
        {
        }

        protected abstract void AddTable(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        protected abstract void AddColumns(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        protected abstract void AddValues(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        protected abstract void AddOutput(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        protected override void Build(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.AppendLine("{0} into", Constants.Insert);
            AddTable(context, sqlBuilder, parameterManager);
            AddColumns(context, sqlBuilder, parameterManager);
            AddValues(context, sqlBuilder, parameterManager);
        }

        protected override void GetParameters(IInsertQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            AddValues(context, sqlBuilder, parameterManager);
        }
    }
}

