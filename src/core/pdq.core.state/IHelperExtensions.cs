using pdq.common;

namespace pdq.state
{
	public interface IHelperExtensions
	{
		IQueryContext Context { get; }
	}

    public class HelperExtensions : IHelperExtensions
    {
        private readonly IQueryContext context;

        public HelperExtensions(IQueryContext context)
        {
            this.context = context;    
        }

        public IQueryContext Context => this.context;
    }
}

