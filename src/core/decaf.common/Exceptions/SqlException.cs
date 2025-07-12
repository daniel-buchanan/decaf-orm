using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions;

public abstract class SqlException : Exception
{
    protected SqlException(string reason, string? lastExecutedSql = null)
        : base(reason)
    {
        LastExecutedSql = lastExecutedSql;
    }

    protected SqlException(Exception innerException, string? reason = null, string? lastExecutedSql = null)
        : base(reason, innerException)
    {
        LastExecutedSql = lastExecutedSql;
    }


    protected SqlException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        
    public string? LastExecutedSql { get; private set; }

    public void AddSql(string sql) => LastExecutedSql = sql;
}