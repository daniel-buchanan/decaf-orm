using System;
namespace decaf.common
{
	public interface IQueryTarget
    {
		string Alias { get; }

		bool IsEquivalentTo(IQueryTarget target);
    }
}

