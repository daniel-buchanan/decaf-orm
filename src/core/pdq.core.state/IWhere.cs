namespace pdq.core.state
{
	public interface IWhere
	{
		IReadOnlyCollection<IWhere> Children { get; }
	}
}

