using System;
namespace pdq.common.Connections
{
	public interface ITransaction : IDisposable
	{
		internal bool CloseTransactionOnCommitOrRollback { get; }

		IConnection Connection { get; }

		void Begin();

		void Commit();

		void Rollback();
	}
}

