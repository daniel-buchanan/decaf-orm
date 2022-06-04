using System;
using pdq.common.Connections;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.common
{
	public interface ITransient : IDisposable
	{
		Guid Id { get; }

		IQuery Query();

		internal IConnection Connection { get; }
	}
}