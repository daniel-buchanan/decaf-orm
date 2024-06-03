namespace decaf.common.Templates;

public class SqlParameter
{
	private SqlParameter(string hash, string name)
	{
		Hash = hash;
		Name = name;
	}

	public static SqlParameter Create(string hash, string name)
		=> new SqlParameter(hash, name);

	public string Hash { get; }

	public string Name { get; }
}