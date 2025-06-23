using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Data;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Exceptions;

namespace decaf.sqlserver.tests;

public class OptionsBuilderTests : SqlServerTest
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
        transactionFactory.Should().BeOfType<SqlServerTransactionFactory>();
    }

    [Fact]
    public void ConnectionFactorySet()
    {
        // Act
        var connectionFactory = provider.GetService<IConnectionFactory>();

        // Assert
        connectionFactory.Should().BeOfType<SqlServerConnectionFactory>();
    }

    [Fact]
    public void SqlFactorySet()
    {
        // Act
        var sqlFactory = provider.GetService<ISqlFactory>();

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
        var builder = new SqlServerOptionsBuilder();

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
        var builder = new SqlServerOptionsBuilder();

        // Act
        builder.UseQuotedIdentifiers();
        var options = builder.Build();

        // Assert
        options.QuotedIdentifiers.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Server=localhost;")]
    [InlineData("Server=localhost,5432;")]
    [InlineData("Server=localhost,5432;Database=db;")]
    [InlineData("Server=localhost,5432;User ID=db;Password=db;")]
    [InlineData("Server=localhost,5432;Database=db;Password=db;")]
    [InlineData("Server=localhost,5432;User ID=db;Database=db;")]
    public void InvalidConnectionString(string connectionString)
    {
        // Arrange
        var builder = new SqlServerOptionsBuilder();

        // Act
        Action method = () => builder.WithConnectionString(connectionString);

        // Assert
        method.Should().Throw<ConnectionStringParsingException>();
    }

    [Fact]
    public void ValidConnectionString()
    {
        // Arrange
        var connString = "Server=xyz,5432;Database=db;User ID=postgres;Password=password;";
        var builder = new SqlServerOptionsBuilder();

        // Act
        builder.WithConnectionString(connString);
        var options = builder.Build();

        // Assert
        options.ConnectionDetails.Hostname.Should().Be("xyz");
        options.ConnectionDetails.Port.Should().Be(5432);
        options.ConnectionDetails.DatabaseName.Should().Be("db");
        options.ConnectionDetails.Authentication.Should().BeOfType<UsernamePasswordAuthentication>();
    }
}