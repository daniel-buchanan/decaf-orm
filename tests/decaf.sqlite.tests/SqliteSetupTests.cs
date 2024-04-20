using System;
using System.Reflection.Metadata;
using decaf.common.Connections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using decaf.tests.common.Models;
using Xunit;

namespace decaf.sqlite.tests
{
	public class SqliteSetupTests
	{
        protected IServiceProvider provider;
        protected readonly IServiceCollection services;

		public SqliteSetupTests()
		{
            services = new ServiceCollection();
            services.AddDecaf(o =>
            {
                o.TrackUnitsOfWork()
                    .OverrideDefaultLogLevel(LogLevel.Debug)
                    .UseSqlite(options =>
                    {
                        options.WithConnectionDetails(new SqliteConnectionDetails()
                        {
                            Authentication = new UsernamePasswordAuthentication("bob", "password")
                        });
                    });
            });
        }

        protected void BuildServiceProvider() => provider = services.BuildServiceProvider();

        [Fact]
        public void CannotSetInMemoryAfterGettingDefaultValue()
        {
            // Arrange
            var details = new SqliteConnectionDetails();
            details.InMemory = false;

            // Act
            Action method = () => details.InMemory = true;

            // Assert
            details.InMemory.Should().BeFalse();
            method.Should().Throw<ConnectionModificationException>();
        }

        [Fact]
        public void CanSetNew()
        {
            // Arrange
            var details = new SqliteConnectionDetails();

            // Act
            details.CreateNew = true;
            details.FullUri = "/Users/buchanand/MyDb.db";

            // Assert
            details.GetConnectionString().Should().Be("Data Source=/Users/buchanand/MyDb.db;Version=2;New=True;");
        }
        
        [Fact]
        public void NewDefaultsToFalse()
        {
            // Arrange
            var details = new SqliteConnectionDetails();

            // Act
            details.FullUri = "/Users/buchanand/MyDb.db";

            // Assert
            details.GetConnectionString().Should().Be("Data Source=/Users/buchanand/MyDb.db;Version=2;");
        }

        [Fact]
        public void CanSetInMemory()
        {
            // Arrange
            var details = new SqliteConnectionDetails();

            // Act
            details.InMemory = true;

            // Assert
            details.GetConnectionString().Should().Be("Data Source=:memory:;Version=2;New=True;");
        }
    }
}

