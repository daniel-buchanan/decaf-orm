using pdq.core.common;

namespace pdq.core.state
{
	public class OrderBy : Column
	{
		private OrderBy(Column column, SortOrder order)
			: base(column.Name, column.Table)
        {
			Order = order;
        }

		public SortOrder Order { get; }

		public static OrderBy Create(string name, Table table, SortOrder order)
        {
			return new OrderBy(Column.Create(name, table), order);
        }

		public static OrderBy Create(Column column, SortOrder order)
        {
			return new OrderBy(column, order);
        }
	}
}

