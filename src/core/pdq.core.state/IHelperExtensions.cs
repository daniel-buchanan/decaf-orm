namespace pdq.state
{
	internal interface IHelperExtensions
	{
		IQueryContextInternal Context { get; }
	}

    internal class HelperExtensions : IHelperExtensions
    {
        private readonly IQueryContextInternal context;

        public HelperExtensions(IQueryContextInternal context)
        {
            this.context = context;    
        }

        IQueryContextInternal IHelperExtensions.Context => this.context;
    }
}

