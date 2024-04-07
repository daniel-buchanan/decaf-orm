namespace decaf.common.Connections
{
    public enum TransactionState
    {
        Created,
        Begun,
        Committed,
        CommitFailed,
        RolledBack,
        RollbackFailed,
        Disposed
    }
}