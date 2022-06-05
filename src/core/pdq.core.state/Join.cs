namespace pdq.state
{
	public class Join
	{
		private Join(IQueryTarget from, IQueryTarget to, JoinType type, IWhere conditions)
		{
			From = from;
			To = to;
			Type = type;
			Conditions = conditions;
		}

		public IQueryTarget From { get; private set; }

		public IQueryTarget To { get; private set; }

		public JoinType Type { get; private set; }

		public IWhere Conditions { get; private set; }

		public static Join Create(
			IQueryTarget from,
			IQueryTarget to,
			JoinType type,
			IWhere conditions)
			=> new Join(from, to, type, conditions);
	}
}

