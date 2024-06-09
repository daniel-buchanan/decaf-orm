namespace decaf.common.Templates;

public static class ParameterWrapper
{
	public static ParameterWrapper<T> Create<T>(T state, object value)
		=> new ParameterWrapper<T>(state, value);
}

public class ParameterWrapper<T>
{
	public ParameterWrapper(T state, object value)
	{
		State = state;
		Value = value;
	}

	public T State { get; }

	public object Value { get; }
}