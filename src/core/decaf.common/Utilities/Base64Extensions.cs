using System;
using System.Text;

namespace decaf.common.Utilities
{
	public static class Base64Extensions
	{
		public static string ToBase64String(this string value)
        {
			var plainTextBytes = Encoding.UTF8.GetBytes(value);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string FromBase64String(this string value)
        {
			var base64EncodedBytes = Convert.FromBase64String(value);
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}

