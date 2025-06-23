using System;
using System.Runtime.Serialization;

namespace decaf.Exceptions;

[Serializable]
public class TableNotFoundException : Exception
{
	public TableNotFoundException(string table)
		: base($"The specified table {table} is not known. Have you forgotten to do a From<T>() or has a join be missed?")
	{

	}

	public TableNotFoundException(string alias, string name)
		: base($"The specified table, with alias: {alias} and name: {name} is not known. Have you forgotten to do a From<T>() or has a join be missed?")
	{
	}

	protected TableNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context) { }
}