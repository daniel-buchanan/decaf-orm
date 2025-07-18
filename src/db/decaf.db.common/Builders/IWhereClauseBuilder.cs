﻿using decaf.common;
using decaf.common.Templates;

namespace decaf.db.common.Builders;

public interface IWhereClauseBuilder
{
	void AddWhere(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
	void AddJoin(IWhere clause, ISqlBuilder sqlBuilder, IParameterManager parameterManager);
}