using System;
using System.Collections.Generic;
using pdq.common.Templates;

namespace pdq.common.Templates
{
	public interface IParameterManager
	{
		void Clear();

		SqlParameter Add<T>(T state, object value);

		IEnumerable<SqlParameter> GetParameters();

		Dictionary<string, object> GetParameterValues();
	}
}

