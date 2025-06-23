using decaf.common;

namespace decaf.state.QueryTargets;

public class SelectQueryTarget : ISelectQueryTarget
{
	private SelectQueryTarget(ISelectQueryContext context, string alias)
	{
		Context = context;
		Alias = alias;
	}

	public string Alias { get; private set; }

	public ISelectQueryContext Context { get; private set; }

	public static IQueryTarget Create(ISelectQueryContext context, string alias) => new SelectQueryTarget(context, alias);

	public bool IsEquivalentTo(IQueryTarget target)
	{
		return target.Alias == Alias;
	}
}