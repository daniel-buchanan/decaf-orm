using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using Xunit;

namespace pdq.core_tests
{
    public class SelectResultTests
    {
        private IQueryInternal query;

        public SelectResultTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IUnitOfWork>();
            var transient = uow.Begin();
            this.query = transient.Query() as IQueryInternal;
        }

        [Fact]
        public void ParseDynamicSucceeds()
        {
            // Arrange
            var interim = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.ClauseHandling.DefaultToAnd();
                    b.Column("first_name", "u").Is().StartsWith("daniel");
                });

            // Act
            Action method = () =>
            {
                interim.Select(b => new
                {
                    Id = b.Is("id", "u"),
                    FirstName = b.Is("first_name", "u"),
                    LastName = b.Is("last_name", "u")
                });
            };

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void ParseConcreteSucceeds()
        {
            // Arrange
            var interim = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.ClauseHandling.DefaultToAnd();
                    b.Column("first_name", "u").Is().StartsWith("daniel");
                });

            // Act
            Action method = () =>
            {
                interim.Select(b => new Person
                {
                    Id = b.Is<int>("id", "u"),
                    FirstName = b.Is<string>("first_name", "u"),
                    LastName = b.Is<string>("last_name", "u")
                });
            };

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void ParseConcreteWithExtensionsSucceeds()
        {
            // Arrange
            var interim = this.query.Select()
                .From<Person>(p => p)
                .Where(p => p.FirstName.Contains("smith"));

            // Act
            Action method = () =>
            {
                interim.Select(b => new Person
                {
                    Id = b.Id,
                    FirstName = b.FirstName.ToLower(),
                    LastName = b.LastName.ToUpper()
                });
            };

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void ParseConcreteWithSubstringSucceeds()
        {
            // Arrange
            var interim = this.query.Select()
                .From<Person>(p => p)
                .Where(p => p.FirstName.Contains("smith"));

            // Act
            Action method = () =>
            {
                interim.Select(b => new Person
                {
                    Id = b.Id,
                    FirstName = b.FirstName.Substring(1),
                    LastName = b.LastName
                });
            };

            // Assert
            method.Should().NotThrow();
        }
    }
}

