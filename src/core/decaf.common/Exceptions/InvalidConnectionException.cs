using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions;

[Serializable]
public class InvalidConnectionException : Exception
{
    public InvalidConnectionException(string reason)
        : base(reason) { }


    protected InvalidConnectionException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}