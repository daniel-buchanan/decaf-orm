using pdq.common;

namespace pdq
{
	public static class QueryExtensions
	{
		public static IDelete Delete(this IQuery query) => Implementation.Delete.Create(query);

		public static ISelect Select(this IQuery query) => Implementation.Select.Create(query);
	}
}

