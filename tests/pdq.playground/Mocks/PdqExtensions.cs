using System;
namespace pdq.playground.Mocks
{
	public static class PdqExtensions
	{
		public static void UseMockDatabase(this PdqOptions options)
        {
			options.ConnectionFactoryType = typeof(MockConnectionFactory);
			options.TransactionFactoryType = typeof(MockTransactionFactory);
        }
	}
}

