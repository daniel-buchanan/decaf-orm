using System;
using System.Collections.Generic;
using decaf.common.Templates;

namespace decaf.common.Templates
{
	public interface IParameterManager
	{
		void Clear();

		SqlParameter Add<T>(T state, object value);

		IEnumerable<SqlParameter> GetParameters();

		Dictionary<string, object> GetParameterValues();
	}
}

