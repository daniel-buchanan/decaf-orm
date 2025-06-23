using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions;

[Serializable]
public class MissingConnectionDetailsException : Exception
{
    public MissingConnectionDetailsException(string reason)
        : base(reason) { }


    protected MissingConnectionDetailsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}