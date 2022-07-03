using System;
using System.Data;

namespace pdq.common.Connections
{
	public interface ITransaction : IDisposable
	{
		internal bool CloseConnectionOnCommitOrRollback { get; }

		Guid Id { get; }

		IConnection Connection { get; }

		void Begin();

		void Commit();

		void Rollback();

		IDbTransaction GetUnderlyingTransaction();
	}
}

