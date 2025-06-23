using System;

namespace decaf.common.Attributes;
[AttributeUsage(AttributeTargets.Property)]

public class RenameColumnAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}