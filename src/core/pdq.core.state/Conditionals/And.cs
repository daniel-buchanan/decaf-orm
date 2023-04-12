using System.Collections.Generic;
using System.Linq;
using pdq.common;

namespace pdq.state.Conditionals
{
	public class And : Where
	{
		private And(params IWhere[] children) : base(children) { }

		public static And Where(params IWhere[] items)
        {
			return new And(items);
        }

        public static And Where(IEnumerable<IWhere> items)
        {
            return new And(items.ToArray());
        }
    }
}

