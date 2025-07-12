using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions;

[Serializable]
public class TemplateParsingException : Exception
{
    public TemplateParsingException(string reason)
        : base(reason) { }


    protected TemplateParsingException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}