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
			Type expression,
			string alias)
		{
            Name = name;
			NewName = newName;
			Value = value;
			Type = expression;
			Alias = alias;
        }

		public static DynamicPropertyInfo Create(
			string name = null,
			string newName = null,
			object value = null,
			Type type = null,
			string alias = null)
			=> new DynamicPropertyInfo(name, newName, value, type, alias);

		public string Name { get; private set; }

		public string NewName { get; private set; }

		public object Value { get; }

		public Type Type { get; }

		public string Alias { get; set; }

        public void SetName(string value) => Name = value;

        public void SetNewName(string value) => NewName = value;

		public bool IsEquivalentTo(DynamicPropertyInfo propertyInfo)
        {
			var equal = Name == propertyInfo.Name &&
				NewName == propertyInfo.NewName &&
				Alias == propertyInfo.Alias;

			if (equal) return true;

			equal = equal ||
				Type == propertyInfo.Type;

			return equal;
        }
	}
}

