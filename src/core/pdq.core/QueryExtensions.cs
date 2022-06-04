using pdq.common;

namespace pdq
{
	public static class QueryExtensions
	{
		public static IDelete Delete(this IQuery query) => Implementation.Delete.Delete.Create(query);
	}
}

