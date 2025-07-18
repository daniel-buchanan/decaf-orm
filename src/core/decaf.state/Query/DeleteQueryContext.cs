﻿using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Utilities;

namespace decaf.state;

internal class DeleteQueryContext :
	QueryContext,
	IDeleteQueryContext
{
	private readonly List<Output> outputs;

	private DeleteQueryContext(
		IAliasManager aliasManager,
		IHashProvider hashProvider)
		: base(aliasManager, QueryTypes.Delete, hashProvider)
	{
		WhereClause = null;
		outputs = new List<Output>();
	}

	/// <inheritdoc/>
	public ITableTarget? Table => QueryTargets.FirstOrDefault()?.CastAs<ITableTarget>();

	/// <inheritdoc/>
	public IWhere? WhereClause { get; private set; }

	/// <inheritdoc/>
	public IReadOnlyCollection<Output> Outputs => outputs.AsReadOnly();

	/// <inheritdoc/>
	public void From(ITableTarget target)
	{
		var item = QueryTargets.FirstOrDefault(t => t.IsEquivalentTo(target));
		if (item != null) return;

		var internalContext = this as IQueryContextExtended;
		internalContext.AddQueryTarget(target);
	}

	/// <inheritdoc/>
	public void Where(IWhere where)
	{
		WhereClause = where;
	}

	/// <inheritdoc/>
	public void Output(Output output)
	{
		outputs.Add(output);
	}

	/// <summary>
	/// Create a <see cref="IDeleteQueryContext"/> Context.
	/// </summary>
	/// <param name="aliasManager">The <see cref="IAliasManager"/> to use.</param>
	/// <returns>A new instance which implements <see cref="IDeleteQueryContext"/>.</returns>
	public static IDeleteQueryContext Create(IAliasManager aliasManager, IHashProvider hashProvider)
		=> new DeleteQueryContext(aliasManager, hashProvider);

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		WhereClause = null;
	}
}