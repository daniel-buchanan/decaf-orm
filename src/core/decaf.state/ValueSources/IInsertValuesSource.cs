using System.Collections.Generic;

namespace decaf.state;

public interface IInsertValuesSource { }

public interface IInsertStaticValuesSource : IInsertValuesSource
{
    void AddValue(object[] value);
    IReadOnlyCollection<object[]> Values { get; }
}

public interface IInsertQueryValuesSource : IInsertValuesSource
{
    ISelectQueryContext Query { get; }
}