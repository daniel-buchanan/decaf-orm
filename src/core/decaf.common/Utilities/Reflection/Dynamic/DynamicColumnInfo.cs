using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace decaf.common.Utilities.Reflection.Dynamic
{
	public class DynamicColumnInfo : IComparable<DynamicColumnInfo>
	{
		private DynamicColumnInfo(
			string name,
			string newName,
			object value,
			Type valueType,
			Type type,
			string alias,
			IValueFunction function)
		{
            Name = name;
			NewName = newName;
			Value = value;
			ValueType = valueType;
			Type = type;
			Alias = alias;
			Function = function;
        }

		public static DynamicColumnInfo Empty()
			=> new DynamicColumnInfo(null, null, null, null, null, null, null);

		public static DynamicColumnInfo Create(
			string name = null,
			string newName = null,
			object value = null,
			Type valueType = null,
			Type type = null,
			string alias = null,
			IValueFunction function = null)
			=> new DynamicColumnInfo(name, newName, value, valueType, type, alias, function);

		public string Name { get; private set; }

		public string NewName { get; private set; }

		public object Value { get; private set; }

		public Type ValueType { get; private set; }

		public Type Type { get; private set; }

		public string Alias { get; private set; }

		public IValueFunction Function { get; private set; }

        public void SetName(string value) => Name = value;

        public void SetNewName(string value) => NewName = value;

		public void SetValue(object value) => Value = value;

		public void SetValueType(Type type) => ValueType = type;

		public void SetType(Type type) => Type = type;

		public void SetAlias(string alias) => Alias = alias;

		public void SetFunction(IValueFunction function) => Function = function;

		public bool IsEquivalentTo(DynamicColumnInfo columnInfo)
        {
			if (columnInfo == null) return false;

			return Name == columnInfo.Name &&
				NewName == columnInfo.NewName &&
				Alias == columnInfo.Alias &&
				Type == columnInfo.Type;
        }

        public int CompareTo(DynamicColumnInfo other)
			=> IsEquivalentTo(other) ? 1 : 0;

        public override bool Equals(object obj)
        {
            return obj is DynamicColumnInfo info &&
                   Name == info.Name &&
                   NewName == info.NewName &&
                   Alias == info.Alias;
        }

        public override int GetHashCode()
        {
            int hashCode = 638763575;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NewName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Alias);
            return hashCode;
        }

        private static int Compare(DynamicColumnInfo left, DynamicColumnInfo right)
			=> left.CompareTo(right);

        public static bool operator ==(DynamicColumnInfo left, DynamicColumnInfo right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(DynamicColumnInfo left, DynamicColumnInfo right)
			=> !(left == right);

        public static bool operator >(DynamicColumnInfo left, DynamicColumnInfo right)
			=> Compare(left, right) > 0;
        
        public static bool operator >=(DynamicColumnInfo left, DynamicColumnInfo right)
	        => Compare(left, right) > 0;

        public static bool operator <(DynamicColumnInfo left, DynamicColumnInfo right)
			=> Compare(left, right) < 0;
        
        public static bool operator <=(DynamicColumnInfo left, DynamicColumnInfo right)
	        => Compare(left, right) < 0;
    }
}

