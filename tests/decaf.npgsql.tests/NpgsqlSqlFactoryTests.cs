using System;
using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Templates;
using decaf.common.Utilities;
using decaf.services;
using decaf.tests.common.Mocks;
using decaf.tests.common.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using decaf.core_tests.Helpers;
using Xunit;

namespace decaf.npgsql.tests
{
    public class NpgsqlSqlFactoryTests : NpgsqlTest
    {
        private readonly ISqlFactory sqlFactory;
        private readonly IService<Person> personService;

        public NpgsqlSqlFactoryTests() : base()
        {
            services.Replace<IConnectionFactory, MockConnectionFactory>();
            services.Replace<ITransactionFactory, MockTransactionFactory>();
            services.AddScoped<IConnectionDetails>(s => new MockConnectionDetails());
            services.AddDecafService<Person>().AsScoped();

            BuildServiceProvider();

            this.sqlFactory = provider.GetService<ISqlFactory>();
            this.personService = provider.GetService<IService<Person>>();
        }

        [Fact]
        public void TemplateParsingSucceeds()
        {
            // Arrange
            var expected = "select\r\n  t.Id,\r\n  t.FirstName,\r\n  t.LastName,\r\n  t.Email,\r\n  t.AddressId,\r\n  t.CreatedAt\r\nfrom\r\n  Person as t\r\nwhere\r\n(t.Email = @p1)\r\n";
            expected = expected.Replace("\r\n", Environment.NewLine);
            IQueryContext context = null;
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Find(p => p.Email == "bob@bob.com");

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
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Find(p => p.Email == "bob@bob.com");

            // Act
            Action method = () => this.sqlFactory.ParseParameters(context, new SqlTemplate("", new List<SqlParameter>()));

            // Assert
            method.Should().Throw<common.Exceptions.SqlTemplateMismatchException>();
        }

        [Fact]
        public void ParameterParsingSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Find(p => p.Email == "bob@bob.com");
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template, includePrefix: false);

            // Assert
            parameters.Should().BeEquivalentTo(ParameterMapper.Map(new Dictionary<string, object>
            {
                { "p1", "bob@bob.com" }
            }));
        }

        [Fact]
        public void ComplexParameterParsingSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Find(u => u.Id == 42 && u.Email.EndsWith(".com") && (u.Id != 0 || u.Email.EndsWith("abc")));
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template, includePrefix: false);

            // Assert
            parameters.Should().BeEquivalentTo(ParameterMapper.Map(new Dictionary<string, object>
            {
                { "p1", 42 },
                { "p2", ".com" },
                { "p3", 0 },
                { "p4", "abc" }
            }));
        }

        [Fact]
        public void InValuesParameterParsingSucceeds()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            var ids = new int[] { 1, 2, 3 };
            this.personService.Find(p => ids.Contains(p.Id));
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template, includePrefix: false);

            // Assert
            parameters.Should().BeEquivalentTo(ParameterMapper.Map(new Dictionary<string, object>
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
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Find(p => p.Email.Contains("bob"));
            var template = this.sqlFactory.ParseTemplate(context);

            // Act
            var parameters = this.sqlFactory.ParseParameters(context, template, includePrefix: false);

            // Assert
            parameters.Should().BeEquivalentTo(ParameterMapper.Map(new Dictionary<string, object>
            {
                { "p1", "bob" }
            }));
        }
    }
}

