using System.Collections.Generic;

namespace decaf.common.Templates;

public interface IParameterManager
{
	void Clear();

	SqlParameter Add<T>(T state, object value);

	IEnumerable<SqlParameter> GetParameters();

	Dictionary<string, object> GetParameterValues(bool includePrefix = false);
}