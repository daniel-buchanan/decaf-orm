using System;
namespace pdq.core.Connections
{
	public interface IConnection : IDisposable, IAsyncDisposable
	{
		ITransaction CreateTransaction();

		void Open();

		void Close();

		internal string GetHash();
	}
}

