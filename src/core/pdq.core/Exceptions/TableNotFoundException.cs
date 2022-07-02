using System;
namespace pdq.Exceptions
{
	public class TableNotFoundException : Exception
	{
		public TableNotFoundException(string alias, string name)
			: base($"The specified table, with alias: {alias} and name: {name} is not known. Have you forgotten to do a From<T>() or has a join be missed?")
		{
		}
	}
}

