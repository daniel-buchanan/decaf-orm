using System;
using System.Collections.Generic;

namespace pdq.common
{
	public static class ListExtensions
	{
		public static void DisposeAll<T>(this List<T> list)
        {
			if(typeof(T).GetMethod("Dispose") != null)
            {
				foreach(var item in list)
                {
                    var disposableItem = (IDisposable)item;
                    disposableItem.Dispose();
                }
            }

            list.Clear();
        }
    }
}

