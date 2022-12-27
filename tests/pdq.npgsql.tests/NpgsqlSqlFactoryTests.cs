using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Utilities;
using pdq.core_tests.Helpers;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using pdq.services;
using Xunit;

namespace pdq.npgsql.tests
{
	public class NpgsqlSqlFactoryTests
	{
        private readonly ISqlFactory sqlFactory;
        private readonly IService<Person> personService;

		public NpgsqlSqlFactoryTests()
		{
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.DisableSqlHeaderComments();
                o.UseNpgsql(options =>
                {

                });
            });
            services.Replace<IConnectionFactory, MockConnectionFactory>();
            services.Replace<ITransactionFactory, MockTransactionFactory>();
            services.AddScoped<IConnectionDetails>(s => new MockConnectionDetails());
            services.AddPdqService<Person>().AsScoped();

            var provider = services.BuildServiceProvider();
            this.sqlFactory = provider.GetService<ISqlFactory>();
            this.personService = provider.GetService<IService<Person>>();
        }

        [Fact]
        public void TemplateParsingSucceeds()
        {
            // Arrange
            var expected = "select\r\n  t.Id,\r\n  t.FirstName,\r\n  t.LastName,\r\n  t.Email,\r\n  t.AddressId,\r\n  t.CreatedAt\r\nfrom\r\n  Person as t\r\nwhere\r\n(\r\n  t.Email = '@p1'\r\n)\r\n";
            expected = expected.Replace("\r\n", Environment.NewLine);
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Get(p => p.Email == "bob@bob.com");

            // Act
            var template = this.sqlFactory.ParseTemplate(context);

            // Assert
            template.Sql.Should().Be(expected);
        }

        [Fact]
        public void ParameterParsingFails()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Get(p => p.Email == "bob@bob.com");

            // Act
            Action method = () => this.sqlFactory.ParseParameters(context, new common.Templates.SqlTemplate("", new List<common.Templates.SqlParameter>()));

            // Assert
            method.Should().Throw<common.Exceptions.SqlTemplateMismatchException>();
        }

        [Fact]
        public void ParameterParsingSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Get(p => p.Email == "bob@bob.com");
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template);

            // Assert
            var dict = parameters as DynamicDictionary;
            dict.Should().BeEquivalentTo(DynamicDictionary.FromDictionary(new Dictionary<string, object>
            {
                { "p1", "bob@bob.com" }
            }));
        }

        [Fact]
        public void InValuesParameterParsingSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };
            var ids = new int[] { 1, 2, 3 };
            this.personService.Get(p => ids.Contains(p.Id));
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template);

            // Assert
            var dict = parameters as DynamicDictionary;
            dict.Should().BeEquivalentTo(DynamicDictionary.FromDictionary(new Dictionary<string, object>
            {
                { "p1", 1 },
                { "p2", 2 },
                { "p3", 3 }
            }));
        }

        [Fact]
        public void LikeParameterParsingSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Get(p => p.Email.Contains("bob"));
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template);

            // Assert
            var dict = parameters as DynamicDictionary;
            dict.Should().BeEquivalentTo(DynamicDictionary.FromDictionary(new Dictionary<string, object>
            {
                { "p1", "bob" }
            }));
        }
    }
}

