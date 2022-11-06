using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using pdq.services;
using pdq.state;
using pdq.state.ValueSources.Insert;
using Xunit;

namespace pdq.core_tests.Services.Command
{
    public class CommandServiceTests
    {
        private readonly IService<Person> personService;

        public CommandServiceTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });
            services.AddPdqService<Person>().AsScoped();
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();

            var provider = services.BuildServiceProvider();
            this.personService = provider.GetService<IService<Person>>();
        }

        [Fact]
        public void AddSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.personService.Add(new Person
            {
                Email = "bob@bob.com"
            });

            // Assert
            context.Should().NotBeNull();
            var insertContext = context as IInsertQueryContext;
            insertContext.Source.Should().BeOfType<StaticValuesSource>();
        }
    }
}

