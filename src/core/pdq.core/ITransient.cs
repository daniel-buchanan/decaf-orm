using System;
namespace pdq.core
{
	public interface ITransient : IDisposable
	{
		IQuery Query();
	}
}