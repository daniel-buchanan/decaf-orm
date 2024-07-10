using System;
using System.Collections.Generic;
using System.Linq;

namespace decaf.state.ValueSources.Insert
{
	public class StaticValuesSource : IInsertStaticValuesSource
	{
		private readonly List<object[]> values;

		private StaticValuesSource(int width, IEnumerable<object[]> values = null)
		{
			Width = width;
			this.values = values?.ToList() ?? new List<object[]>();
		}

		public int Width { get; private set; }

		public IReadOnlyCollection<object[]> Values => values.AsReadOnly();

		public void AddValue(object[] value)
        {
			if (value == null) throw new ArgumentNullException(nameof(value), "The values MUST be provided when adding to a StaticValueSource.");
			if (value.Length < Width || value.Length > Width) throw new ArgumentOutOfRangeException(nameof(value), $"The values provided MUST have {Width} values in the tuple.");

			values.Add(value);
        }

        public static StaticValuesSource Create(int width) => new StaticValuesSource(width);
	}
}
