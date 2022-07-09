using System;
using System.Linq.Expressions;

namespace pdq.state.Utilities
{
	internal interface IParser
	{
		public IWhere Parse(Expression expression);
	}
}

