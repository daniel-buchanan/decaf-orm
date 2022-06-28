using pdq.common;

namespace pdq.state
{
    public class GroupBy : Column
    {
        private GroupBy(string name, IQueryTarget source)
            : base(name, source)
        {
        }

        public static new GroupBy Create(string name, IQueryTarget source) => new GroupBy(name, source);

    }
}

