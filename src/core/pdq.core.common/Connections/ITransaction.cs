using System;
using System.Data;

namespace pdq.common.Connections
{
	public interface ITransaction : IDisposable
	{
		Guid Id { get; }

		IConnection Connection { get; }

		void Begin();

		void Commit();

		void Rollback();

		IDbTransaction GetUnderlyingTransaction();
	}

	internal interface ITransactionInternal : ITransaction
    {
		bool CloseConnectionOnCommitOrRollback { get; }
	}
}

