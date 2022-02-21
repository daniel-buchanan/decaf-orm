using System;
using System.Collections.Generic;
using pdq.core.Where;

namespace pdq.core
{
	public abstract class AbstractWhereItem : IWhereItem
	{
		protected readonly List<IWhereItem> children;
		internal WhereItemType type;

		internal AbstractWhereItem(WhereItemType type)
        {
			this.type = type;
			this.children = new List<IWhereItem>();
		}

		public IEnumerable<IWhereItem> Children => this.children;
    }
}

