using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Connections;
using pdq.tests.common.Mocks;
using pdq.tests.common.Models;
using Xunit;

namespace pdq.core_tests
{
    public class SelectResultTests
    {
        private IQueryContainerInternal query;

        public SelectResultTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IUnitOfWork>();
            var transient = uow.Begin();
            this.query = transient.Query() as IQueryContainerInternal;
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
                interim.Select(p => new Person
                {
                    Id = p.Id,
                    FirstName = p.FirstName.ToLower(),
                    LastName = p.LastName.ToUpper()
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
                interim.Select(p => new Person
                {
                    Id = p.Id,
                    FirstName = p.FirstName.Substring(1),
                    LastName = p.LastName
                });
            };

            // Assert
            method.Should().NotThrow();
        }
    }
}

