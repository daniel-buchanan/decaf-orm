namespace pdq
{
	public interface ISelectColumn<T>
	{
		T Column(string name, string tableAlias = null, string newName = null);
	}
}

