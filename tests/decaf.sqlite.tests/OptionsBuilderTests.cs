using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
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
			var transactionFactory = provider.GetService<ITransactionFactory>();

			// Assert
			transactionFactory.Should().BeOfType<SqliteTransactionFactory>();
		}

        [Fact]
        public void ConnectionFactorySet()
        {
            // Act
            var connectionFactory = provider.GetService<IConnectionFactory>();

            // Assert
            connectionFactory.Should().BeOfType<SqliteConnectionFactory>();
        }

        [Fact]
        public void SqlFactorySet()
        {
            // Act
            var sqlFactory = provider.GetService<ISqlFactory>();

            // Assert
            sqlFactory.Should().BeOfType<db.common.SqlFactory>();
        }
        
        [Fact]
        public void CreateNewSet()
        {
            // Arrange
            var builder = new SqliteOptionsBuilder();

            // Act
            builder.CreateNewDatabase();
            var options = builder.Build();

            // Assert
            options.CreateNew.Should().BeTrue();
        }
        
        [Fact]
        public void DatabasePathSet()
        {
            // Arrange
            const string path = "/Users/buchanand/MyDb.db";
            var builder = new SqliteOptionsBuilder();

            // Act
            builder.WithFilePath(path);
            var options = builder.Build();

            // Assert
            options.DatabasePath.Should().Be(path);
        }
        
        [Fact]
        public void DefaultVersionSet()
        {
            // Arrange
            var builder = new SqliteOptionsBuilder();

            // Act
            var options = builder.Build();

            // Assert
            options.ReadOnly.Should().BeFalse();
        }
        
        [Fact]
        public void VersionSet()
        {
            // Arrange
            var builder = new SqliteOptionsBuilder();

            // Act
            builder.AsReadonly();
            var options = builder.Build();

            // Assert
            options.ReadOnly.Should().BeTrue();
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
            var connString = "Data Source=MyDb.db;Mode=ReadOnly;";
            var builder = new SqliteOptionsBuilder();

            // Act
            builder.WithConnectionString(connString);
            var options = builder.Build();

            // Assert
            options.DatabasePath.Should().Be("MyDb.db");
            options.ReadOnly.Should().BeTrue();
            options.CreateNew.Should().BeFalse();
        }
    }
}

