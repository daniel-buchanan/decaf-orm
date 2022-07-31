using System;
using System.Data;


[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.common.Connections
{
	public interface IConnection : IDisposable
	{
		void Open();

		void Close();

		IDbConnection GetUnderlyingConnection();

		TConnection GetUnderlyingConnectionAs<TConnection>() where TConnection: IDbConnection;
	}

	internal interface IConnectionInternal : IConnection
    {
		string GetHash();
	}
}

