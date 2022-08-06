using System.Collections.Generic;

namespace pdq.state
{
    public interface IInsertValuesSource { }

    public interface IInsertStaticValuesSource : IInsertValuesSource
    {
        void AddValue(object[] value);
        IReadOnlyCollection<object[]> Values { get; }
    }
}