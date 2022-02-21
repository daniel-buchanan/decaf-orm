using System;
namespace pdq.core
{
	public interface ISelectColumn<T>
	{
		T Column(string name, string? tableAlias = null, string? newName = null);
	}
}

