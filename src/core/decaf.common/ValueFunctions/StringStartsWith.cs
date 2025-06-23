namespace decaf.common.ValueFunctions;

public class StringStartsWith : ValueFunction<string>
{
    private StringStartsWith(string value)
        : base(ValueFunction.StartsWith)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static StringStartsWith Create(string value) => new StringStartsWith(value);
}