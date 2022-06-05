using pdq.common;

namespace pdq.state
{
	public class OrderBy : Column
	{
		private OrderBy(Column column, SortOrder order)
			: base(column.Name, column.Source)
        {
			Order = order;
        }

		public SortOrder Order { get; }

		public static OrderBy Create(string name, IQueryTarget source, SortOrder order)
        {
			return new OrderBy(Column.Create(name, source), order);
        }

		public static OrderBy Create(Column column, SortOrder order)
        {
			return new OrderBy(column, order);
        }
	}
}

