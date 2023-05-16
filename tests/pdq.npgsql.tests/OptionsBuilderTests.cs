using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;
using Xunit;
using FluentAssertions;
using pdq.common;

namespace pdq.npgsql.tests
{
	public class OptionsBuilderTests : NpgsqlTest
	{
        public OptionsBuilderTests() : base()
        {
            BuildServiceProvider();
        }

		[Fact]
		public void TransactionFactorySet()
		{
			// Act
			var transactionFactory = this.provider.GetService<ITransactionFactory>();

			// Assert
			transactionFactory.Should().BeOfType<NpgsqlTransactionFactory>();
		}

        [Fact]
        public void ConnectionFactorySet()
        {
            // Act
            var connectionFactory = this.provider.GetService<IConnectionFactory>();

            // Assert
            connectionFactory.Should().BeOfType<NpgsqlConnectionFactory>();
        }

        [Fact]
        public void SqlFactorySet()
        {
            // Act
            var sqlFactory = this.provider.GetService<ISqlFactory>();

            // Assert
            sqlFactory.Should().BeOfType<db.common.SqlFactory>();
        }
    }
}

