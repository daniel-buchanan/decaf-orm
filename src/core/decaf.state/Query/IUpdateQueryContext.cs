using System.Collections.Generic;
using decaf.common;

namespace decaf.state;

public interface IUpdateQueryContext : IQueryContext
{
	/// <summary>
	/// 
	/// </summary>
	ITableTarget? Table { get; }

	/// <summary>
	/// 
	/// </summary>
	IReadOnlyCollection<IUpdateValueSource> Updates { get; }

	/// <summary>
	/// 
	/// </summary>
	IQueryTarget? Source { get; }

	/// <summary>
	/// 
	/// </summary>
	IWhere? WhereClause { get; }

	/// <summary>
	/// 
	/// </summary>
	IReadOnlyCollection<Output> Outputs { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="target"></param>
	void Update(ITableTarget target);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="source"></param>
	void From(IQueryTarget source);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="whereClause"></param>
	void Where(IWhere whereClause);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="outputClause"></param>
	void Output(Output outputClause);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	void Set(IUpdateValueSource value);
}