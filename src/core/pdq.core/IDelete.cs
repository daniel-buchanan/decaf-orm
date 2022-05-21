using System;
namespace pdq.core
{
	public interface IDelete
	{
		IDeleteFrom From(string name, string? alias = null, string? schema = null);
	}
}

