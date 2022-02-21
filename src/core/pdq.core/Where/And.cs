using System;
using pdq.core.Where;

namespace pdq.core
{
	public class And : AbstractWhereItem
	{
		internal And() : base(WhereItemType.And) { }

		public And(params IWhereItem[] children) : this()
		{
			if (children == null) return;
			this.children.AddRange(children);
		}

		public static And Where(params IWhereItem[] children) => new And(children);
	}
}

