using System;

namespace decaf.ddl.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class IndexAttribute : Attribute
{
    public string Name { get; set; }

    public IndexAttribute() { }

    public IndexAttribute(string name) => Name = name;
}

[AttributeUsage(AttributeTargets.Property)]
public class IndexComponentAttribute : Attribute
{
    public string IndexName { get; set; }

    public IndexComponentAttribute() { }

    public IndexComponentAttribute(string indexName) => IndexName = indexName;
}