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

		public void AddValue(object[] values)
        {
			if (values == null) throw new ArgumentNullException(nameof(values), "The values MUST be provided when adding to a StaticValueSource.");
			if (values.Length < Width || values.Length > Width) throw new ArgumentOutOfRangeException(nameof(values), $"The values provided MUST have {Width} values in the tuple.");

			this.values.Add(values);
        }

		public static StaticValuesSource Create(int width)
        {
			return new StaticValuesSource(width);
        }

		public static StaticValuesSource Create(IEnumerable<object[]> values)
        {
            var previousWidth = 0;
			foreach(var val in values)
            {
                var currentWidth = val?.Length ?? 0;
                if (currentWidth == 0) throw new ArgumentOutOfRangeException(
					 nameof(values),
					 "At least one of the tuples provided is NULL, ensure that all value tuples are NOT null.");

				if (previousWidth == 0) continue;
				if (previousWidth != currentWidth) throw new ArgumentOutOfRangeException(
					 nameof(values),
					 "At least one of the tuples provided has a different number of values than the others, ensure ALL tuples have the same number of values.");

				 previousWidth = currentWidth;
            }

			return new StaticValuesSource(previousWidth, values);
        }
	}
}
