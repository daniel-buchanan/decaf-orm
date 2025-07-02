using System;

namespace decaf.common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TableNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;

    public bool CaseSensitive { get; set; }
}