using System;
namespace pdq.common
{
	public interface IQueryTarget
    {
		string Alias { get; }

		bool IsEquivalentTo(IQueryTarget target);
    }
}

