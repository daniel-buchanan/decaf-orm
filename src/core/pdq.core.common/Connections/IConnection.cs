using System;
namespace pdq.common.Connections
{
	public interface IConnection : IDisposable, IAsyncDisposable
	{
		ITransaction CreateTransaction();

		void Open();

		void Close();

		internal string GetHash();
	}

	public interface IConnection<T> : IConnection
    {
		T GetUnderlyingConnection();
    }
}

