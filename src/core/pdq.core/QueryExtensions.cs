using pdq.core.common;
using pdq.core.state;

namespace pdq.core
{
	public static class QueryExtensions
	{
		public static IDelete Delete(this IQuery query)
        {
			var context = DeleteQueryContext.Create();
			var iq = (IQueryInternal)query;
			iq.SetContext(context);
			return iq.GetFluent<IDelete>();
        }
	}
}

