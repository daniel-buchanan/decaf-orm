namespace pdq.core
{
	public interface ISelectFrom :
		IBuilder<ISelectFrom>,
		ISelectColumn<ISelectFrom>,
		IJoinTo<ISelectFrom>
	{
		IWhere Where(IWhereItem where);
	}
}