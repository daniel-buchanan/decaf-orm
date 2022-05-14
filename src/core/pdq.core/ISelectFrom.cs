namespace pdq.core
{
	public interface ISelectFrom :
		ISelectColumn<ISelectFrom>,
		IJoinTo<ISelectFrom>
	{
		IWhere Where(IWhereItem where);
	}
}