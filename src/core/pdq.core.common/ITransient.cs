using System;
namespace pdq.core.common
{
	public interface ITransient : IDisposable
	{
		Guid Id { get; }

		IQuery Query();

		internal T GetFluent<T>() where T : IFluentApi, new();
	}
}