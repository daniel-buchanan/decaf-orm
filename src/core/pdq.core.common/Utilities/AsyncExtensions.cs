using System;
using System.Threading.Tasks;

namespace pdq.common.Utilities
{
	internal static class AsyncHelper
	{
		public static void WaitFor(this Task task)
        {
			task.Wait();
        }

		public static T WaitFor<T>(this Task<T> task)
        {
			task.Wait();
			return task.Result;
        }
	}
}

