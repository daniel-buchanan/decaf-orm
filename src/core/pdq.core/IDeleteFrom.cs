namespace pdq.core
{
	public interface IDeleteFrom : IBuilder
	{
		IDeleteFrom Where(state.IWhere where);
	}
}

