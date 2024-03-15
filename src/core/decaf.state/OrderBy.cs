using decaf.common;

namespace decaf.state
{
	public class OrderBy : ColumnBase<OrderBy>
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

        public override bool IsEquivalentTo(OrderBy column)
        {
            return column.Name == Name &&
                column.Source.Alias == Source.Alias;
        }
    }
}

