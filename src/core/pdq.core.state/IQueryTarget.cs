using System;
namespace pdq.state
{
	public interface IQueryTarget
    {
		string Alias { get; }

		bool IsEquivalentTo(IQueryTarget target);
    }

	public interface ITableTarget : IQueryTarget
    {
		string Name { get; }

		string Schema { get; }

		bool IsEquivalentTo(ITableTarget target);
    }
}

