using pdq.common;

namespace pdq
{
	public static class QueryExtensions
	{
		public static IDelete Delete(this IQuery query)
        {
			var internalQuery = (IQueryInternal)query;
            return Implementation.Delete.Delete.Create(internalQuery);
        }
	}
}

