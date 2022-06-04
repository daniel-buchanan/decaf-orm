using System.Collections.Generic;
namespace pdq.core.common
{
	public interface IFluentApiCache
	{
		T Get<T>() where T: IFluentApi, new();

		IReadOnlyList<string> KnownInstances { get; }
	}
}

