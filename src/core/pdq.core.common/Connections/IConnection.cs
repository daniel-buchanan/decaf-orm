using System;
using System.Data;


[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.common.Connections
{
	public interface IConnection : IDisposable, IAsyncDisposable
	{
		ITransaction CreateTransaction();

		void Open();

		void Close();

		internal string GetHash();

		internal IDbConnection GetUnderlyingConnection();

		internal TConnection GetUnderlyingConnectionAs<TConnection>();
	}
}

