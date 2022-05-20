using System;
namespace pdq.core.common
{
	public interface ITransient : IDisposable
	{
		IQuery Query();
	}
}