using System;

namespace decaf.common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class IgnoreColumnForAttribute(QueryTypes toIgnore = QueryTypes.None) : Attribute
{
    public QueryTypes QueryType { get; set; } = toIgnore;
}