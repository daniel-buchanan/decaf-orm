using System;

namespace decaf.common.Utilities.Reflection;

public class AttributeInfo(SourceType source, string appliedTo, Type appliedType)
{
    public SourceType Source { get; private set; } = source;
    public string AppliedTo { get; private set; } = appliedTo;
    public Type AppliedType { get; private set; } = appliedType;
    public object? Attribute { get; protected set; }

    public static AttributeInfo Create(string appliedTo, Type appliedType, SourceType source, object attribute)
        => new(source, appliedTo, appliedType)
        {
            AppliedTo = appliedTo,
            AppliedType = appliedType,
            Source = source,
            Attribute = attribute
        };

    public static AttributeInfo<T> Create<T>(string appliedTo, Type appliedType, SourceType source, T attribute)
        where T : Attribute
        => new(source, appliedTo, appliedType)
        {
            AppliedTo = appliedTo,
            AppliedType = appliedType,
            Source = source,
            Attribute = attribute
        };
}

public class AttributeInfo<T>(SourceType source, string appliedTo, Type appliedType) :
    AttributeInfo(source, appliedTo, appliedType)
    where T : Attribute
{
    public new T? Attribute
    {
        get => base.Attribute?.CastAs<T>();
        set => base.Attribute = value;
    }
}

public enum SourceType
{
    Class,
    Property,
    Field,
    Namespace
}