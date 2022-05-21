namespace pdq.core.state
{
	public class Join
	{
		private Join(Table table, JoinType type, IWhere conditions)
		{
			Table = table;
			Type = type;
			Conditions = conditions;
		}

		public Table Table { get; private set; }

		public JoinType Type { get; private set; }

		public IWhere Conditions { get; private set; }

		public static Join Create(Table table, JoinType type, IWhere conditions)
        {
			return new Join(table, type, conditions);
        }
	}
}

