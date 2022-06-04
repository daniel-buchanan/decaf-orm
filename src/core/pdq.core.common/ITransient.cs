using System;
namespace pdq.common
{
	public interface ITransient : IDisposable
	{
		Guid Id { get; }

		IQuery Query();
	}
}