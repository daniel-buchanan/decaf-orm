using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common.Connections;
using Xunit;
using FluentAssertions;
using pdq.common;
using System.Data;

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

        [Theory]
        [InlineData(IsolationLevel.Chaos)]
        [InlineData(IsolationLevel.ReadCommitted)]
        [InlineData(IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.RepeatableRead)]
        [InlineData(IsolationLevel.Serializable)]
        [InlineData(IsolationLevel.Snapshot)]
        public void IsolationLevelSet(IsolationLevel level)
        {
            // Arrange
            var builder = new NpgsqlOptionsBuilder();

            // Act
            builder.SetIsolationLevel(level);
            var options = builder.Build();

            // Assert
            options.TransactionIsolationLevel.Should().Be(level);
        }

        [Fact]
        public void UseQuotedIdentifiersSet()
        {
            // Arrange
            var builder = new NpgsqlOptionsBuilder();

            // Act
            builder.UseQuotedIdentifiers();
            var options = builder.Build();

            // Assert
            options.QuotedIdentifiers.Should().BeTrue();
        }
    }
}

