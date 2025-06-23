namespace decaf.common.ValueFunctions;

public class Trim : ValueFunction<string>
{
	private Trim()
		: base(ValueFunction.Trim)
	{
	}

	public static Trim Create() => new Trim();
}