using System.Collections.Generic;
using System.Linq;

namespace pdq.state.Conditionals
{
	public class Or : Where
	{
		private Or(params IWhere[] children) : base(children) { }

		public static Or Where(params IWhere[] items)
        {
			return new Or(items);
        }

        public static Or Where(IEnumerable<IWhere> items)
        {
            return new Or(items.ToArray());
        }
    }
}

