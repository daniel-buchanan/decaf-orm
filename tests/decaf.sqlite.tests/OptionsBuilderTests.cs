using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Data;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Exceptions;

namespace decaf.sqlite.tests
{
	public class OptionsBuilderTests : SqliteTest
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
			transactionFactory.Should().BeOfType<SqliteTransactionFactory>();
		}

        [Fact]
        public void ConnectionFactorySet()
        {
            // Act
            var connectionFactory = this.provider.GetService<IConnectionFactory>();

            // Assert
            connectionFactory.Should().BeOfType<SqliteConnectionFactory>();
        }

        [Fact]
        public void SqlFactorySet()
        {
            // Act
            var sqlFactory = this.provider.GetService<ISqlFactory>();

            // Assert
            sqlFactory.Should().BeOfType<db.common.SqlFactory>();
        }

        [Fact]
        public void UseQuotedIdentifiersSet()
        {
            // Arrange
            var builder = new SqliteOptionsBuilder();

            // Act
            builder.UseQuotedIdentifiers();
            var options = builder.Build();

            // Assert
            options.QuotedIdentifiers.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Data Source=localhost;")]
        [InlineData("Server=localhost,5432;")]
        [InlineData("Server=localhost,5432;Database=db;")]
        [InlineData("Server=localhost,5432;User ID=db;Password=db;")]
        [InlineData("Server=localhost,5432;Database=db;Password=db;")]
        [InlineData("Server=localhost,5432;User ID=db;Database=db;")]
        public void InvalidConnectionString(string connectionString)
        {
            // Arrange
            var builder = new SqliteOptionsBuilder();

            // Act
            Action method = () => builder.WithConnectionString(connectionString);

            // Assert
            method.Should().Throw<ConnectionStringParsingException>();
        }

        [Fact]
        public void ValidConnectionString()
        {
            // Arrange
            var connString = "Data Source=MyDb.db;Version=3;New=True;";
            var builder = new SqliteOptionsBuilder();

            // Act
            builder.WithConnectionString(connString);
            var options = builder.Build();

            // Assert
            options.DatabasePath.Should().Be("MyDb.db");
            options.Version.Should().Be(3);
            options.CreateNew.Should().BeTrue();
        }
    }
}

