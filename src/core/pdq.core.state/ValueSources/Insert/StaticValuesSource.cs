using System;
using System.Collections.Generic;
using System.Linq;

namespace pdq.state.ValueSources.Insert
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

		public IReadOnlyCollection<object[]> Values => this.values.AsReadOnly();

		public void AddValue(object[] value)
        {
			if (value == null) throw new ArgumentNullException(nameof(value), "The values MUST be provided when adding to a StaticValueSource.");
			if (value.Length < Width || value.Length > Width) throw new ArgumentOutOfRangeException(nameof(value), $"The values provided MUST have {Width} values in the tuple.");

			this.values.Add(value);
        }

		public static StaticValuesSource Create(int width)
        {
			return new StaticValuesSource(width);
        }

		public static StaticValuesSource Create(IEnumerable<object[]> values)
        {
			var maxWidth = values.Max(v => v.Length);
			var anyDifferentWidths = values.Sum(v => v.Length) / values.Count() == maxWidth;
			var anyNulls = values.Any(v => v == null) || values.Any(v => v?.All(vv => vv != null) == false);

			if(anyNulls) throw new ArgumentOutOfRangeException(
                nameof(values),
                "At least one of the tuples provided is NULL, ensure that all value tuples are NOT null.");

			if(anyDifferentWidths) throw new ArgumentOutOfRangeException(
                nameof(values),
                "At least one of the tuples provided has a different number of values than the others, ensure ALL tuples have the same number of values.");

			return new StaticValuesSource(maxWidth, values);
        }
	}
}
