namespace pdq.state
{
    public interface IInsertValuesSource { }

    public interface IInsertStaticValuesSource : IInsertValuesSource
    {
        void AddValue(object[] value);
    }
}