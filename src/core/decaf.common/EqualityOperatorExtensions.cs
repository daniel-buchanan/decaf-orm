namespace decaf.common;

public static class EqualityOperatorExtensions
{
	public static string ToOperatorString(this EqualityOperator op)
	{
		switch (op)
		{
			case EqualityOperator.Equals: return "=";
			case EqualityOperator.NotEquals: return "!=";
			case EqualityOperator.LessThan: return "<";
			case EqualityOperator.LessThanOrEqualTo: return "<=";
			case EqualityOperator.GreaterThan: return ">";
			case EqualityOperator.GreaterThanOrEqualTo: return ">=";
			case EqualityOperator.Like:
			case EqualityOperator.StartsWith:
			case EqualityOperator.EndsWith: return "like";
			case EqualityOperator.In: return "in";
			case EqualityOperator.Between: return "between";
			default: return null;
		}
	}
}