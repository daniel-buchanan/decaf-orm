namespace pdq.state
{
    public class GroupBy : Column
    {
        private GroupBy(string name, Table table) : base(name, table)
        {
        }

        public static new GroupBy Create(string name, Table table)
        {
            return new GroupBy(name, table);
        }
    }
}

