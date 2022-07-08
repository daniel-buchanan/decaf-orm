using System;
using pdq.common;

namespace pdq.state.QueryTargets
{
	public class TableTarget : ITableTarget
	{
		private TableTarget()
		{
			Id = Guid.NewGuid();
		}

		private TableTarget(string name, string alias, string schema) : this()
		{
			Name = name;
			Alias = alias;
			Schema = schema;
		}

		public Guid Id { get; private set; }

		public string Name { get; private set; }

		public string Alias { get; private set; }

		public string Schema { get; private set; }

		public static TableTarget Create(string name, string alias = null, string schema = null) => new TableTarget(name, alias, schema);

		public bool IsEquivalentTo(ITableTarget source)
		{
			var minimum = source.Name == Name &&
				source.Alias == Alias;

			if (!minimum) return false;

			var sameSchema = source.Schema == Schema;
			return sameSchema;
		}

        public bool IsEquivalentTo(IQueryTarget target)
        {
			return target.Alias == Alias;
        }
    }
}

