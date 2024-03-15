using System.Threading;
using System.Threading.Tasks;

namespace decaf.common.Utilities
{
	public static class AsyncHelper
	{
        public static void WaitFor(
            this Task task,
            CancellationToken cancellationToken = default)
            => task.Wait(cancellationToken);

        public static T WaitFor<T>(
            this Task<T> task,
            CancellationToken cancellationToken = default)
        {
			task.Wait(cancellationToken);
			return task.Result;
        }
        
        public static dynamic WaitFor(
	        this Task<dynamic> task,
	        CancellationToken cancellationToken = default)
        {
	        task.Wait(cancellationToken);
	        return task.Result;
        }
	}
}

