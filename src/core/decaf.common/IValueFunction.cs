using System;

namespace decaf.common;

public interface IValueFunction : IWhere
{
	/// <summary>
	/// 
	/// </summary>
	ValueFunction Type { get; }

	/// <summary>
	/// 
	/// </summary>
	object[] Arguments { get; }

	/// <summary>
	/// 
	/// </summary>
	Type ValueType { get; }
}