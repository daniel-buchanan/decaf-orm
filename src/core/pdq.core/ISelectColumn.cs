namespace pdq
{
	public interface ISelectColumn<T>
	{
		T Column(string name, string table = null, string tableAlias = null, string newName = null);
	}
}

