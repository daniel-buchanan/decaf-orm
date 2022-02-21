using System;
namespace pdq.core
{
	public interface IWhere :
		IBuilder<IWhere>,
		IOrderBy
	{
		
	}
}

