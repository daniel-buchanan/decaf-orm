using System;
using System.Data;


[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.common.Connections
{
	public interface IConnection : IDisposable, IAsyncDisposable
	{
		void Open();

		void Close();

		internal string GetHash();

		IDbConnection GetUnderlyingConnection();

		TConnection GetUnderlyingConnectionAs<TConnection>() where TConnection: IDbConnection;
	}
}

