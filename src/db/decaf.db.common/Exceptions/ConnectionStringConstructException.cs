using System;
using System.Runtime.Serialization;

namespace decaf.db.common.Exceptions;

[Serializable]
public class ConnectionStringConstructException : Exception
{
    public ConnectionStringConstructException(string reason)
        : base(reason) { }


    protected ConnectionStringConstructException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}