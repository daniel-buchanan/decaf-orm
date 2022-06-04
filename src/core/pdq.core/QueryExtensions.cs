using pdq.core.common;
using pdq.core.state;

namespace pdq.core
{
	public static class QueryExtensions
	{
		public static IDelete Delete(this IQuery query)
        {
			var iq = (IQueryInternal)query;
			var context = DeleteQueryContext.Create();
			iq.SetContext(context);
			return iq.GetFluent<IDelete>();
        }
	}
}

