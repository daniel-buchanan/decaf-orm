using decaf.common;

namespace decaf.state;

public interface ITableTarget : IQueryTarget
{
	string Name { get; }

	string Schema { get; }

	bool IsEquivalentTo(ITableTarget target);
}