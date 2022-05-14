using System;
namespace pdq.core
{
	public interface IQuery : IBuilder
	{
		ISelect Select();
	}
}

