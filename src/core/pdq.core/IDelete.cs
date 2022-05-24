using System;
namespace pdq.core
{
	public interface IDelete : IBuilder
	{
		IDeleteFrom From(string name, string? alias = null, string? schema = null);
	}
}

