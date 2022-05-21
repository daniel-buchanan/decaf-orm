namespace pdq.core
{
	public interface ISelectFrom :
		ISelectColumn<ISelectFrom>,
		IJoinTo<ISelectFrom>
	{
		ISelectFrom Where(IWhere where);
	}
}