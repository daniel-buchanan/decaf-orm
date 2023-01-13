﻿using System;
using System.Collections.Generic;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state;

namespace pdq.db.common.Builders
{
	public abstract class SelectBuilder : Builder<ISelectQueryContext>
	{
        public SelectBuilder(
            IWhereBuilder whereBuilder,
            IHashProvider hashProvider,
            PdqOptions pdqOptions)
            : base(whereBuilder, hashProvider, pdqOptions)
        {
        }

		protected abstract void AddColumns(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

		protected abstract void AddTables(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

		protected abstract void AddJoins(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

		protected abstract void AddOrderBy(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

		protected abstract void AddGroupBy(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager);

        private void AddWhere(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
            => this.whereBuilder.AddWhere(context.WhereClause, sqlBuilder, parameterManager);

        protected override void Build(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            sqlBuilder.AppendLine(Constants.Select);
            AddColumns(context, sqlBuilder, parameterManager);
            AddTables(context, sqlBuilder, parameterManager);
            AddJoins(context, sqlBuilder, parameterManager);
            AddWhere(context, sqlBuilder, parameterManager);
            AddOrderBy(context, sqlBuilder, parameterManager);
            AddGroupBy(context, sqlBuilder, parameterManager);
        }

        protected override void GetParameters(ISelectQueryContext context, ISqlBuilder sqlBuilder, IParameterManager parameterManager)
        {
            AddJoins(context, sqlBuilder, parameterManager);
            AddWhere(context, sqlBuilder, parameterManager);
        }
    }
}

