namespace pdq.state
{
	public interface IHelperExtensions
	{
		internal IQueryContextInternal Context { get; }
	}

    public class HelperExtensions : IHelperExtensions
    {
        private readonly IQueryContextInternal context;

        public HelperExtensions(IQueryContextInternal context)
        {
            this.context = context;    
        }

        IQueryContextInternal IHelperExtensions.Context => this.context;
    }
}

