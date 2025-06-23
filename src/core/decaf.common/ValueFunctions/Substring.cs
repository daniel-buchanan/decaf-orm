namespace decaf.common.ValueFunctions;

public class Substring : ValueFunction<string>
{
	private Substring(int start, int? end = null)
		: base(ValueFunction.Substring, start, end)
	{
	}

	public static Substring Create(int start, int? end = null) => new Substring(start, end);
}