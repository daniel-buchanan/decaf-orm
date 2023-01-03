﻿using System;
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
	public class NpgsqlSqlFactoryCommentTest
	{
        private readonly ISqlFactory sqlFactory;
        private readonly IService<Person> personService;

		public NpgsqlSqlFactoryCommentTest()
		{
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
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
        public void QueryHeadersPresent()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Get(p => p.Email == "bob@bob.com");

            // Act
            var template = this.sqlFactory.ParseTemplate(context);
            var sql = template.Sql;

            // Assert
            var lines = sql.Split(Environment.NewLine);
            lines[0].Should().StartWith("-- pdq :: query hash:");
            lines[1].Should().StartWith("-- pdq :: generated at:");
        }
    }
}
