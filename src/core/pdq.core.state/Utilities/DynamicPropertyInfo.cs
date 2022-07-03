using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
	public class DynamicPropertyInfo
	{
		private DynamicPropertyInfo(
			string name,
			string newName,
			object value,
			Type expression)
		{
            Name = name;
			NewName = newName;
			Value = value;
			Type = expression;
        }

		public static DynamicPropertyInfo Create(
			string name = null,
			string newName = null,
			object value = null,
			Type type = null)
			=> new DynamicPropertyInfo(name, newName, value, type);

		public string Name { get; private set; }

		public string NewName { get; private set; }

		public object Value { get; }

		public Type Type { get; }

        public void SetName(string value) => Name = value;

        public void SetNewName(string value) => NewName = value;
	}
}

