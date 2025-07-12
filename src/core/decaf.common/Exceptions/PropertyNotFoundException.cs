using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions;

[Serializable]
public class PropertyNotFoundException : InvalidOperationException
{
    public PropertyNotFoundException(string property)
        : base(property) { }

    public PropertyNotFoundException(string property, string reason)
        : base(GetMessage(property, reason)) { }
        
    public PropertyNotFoundException(string property, Exception innerException, string? reason = null)
        : base(GetMessage(property, reason ?? string.Empty), innerException) { }
        
    public PropertyNotFoundException(Exception innerException, string? reason = null)
        : base(reason, innerException) { }


    protected PropertyNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetMessage(string property, string reason)
    {
        var msg = $"Property \"{property}\" was not found on the provided type.";
        if (!string.IsNullOrWhiteSpace(reason))
            msg = $"{msg} {reason}";
        return msg;
    }
}