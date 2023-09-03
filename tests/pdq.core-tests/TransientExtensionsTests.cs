using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.services;
using pdq.tests.common.Mocks;
using Xunit;
using pdq.tests.common.Models;
using pdq.common.Connections;

namespace pdq.core_tests
{
    public class TransientExtensionsTests
    {
        private readonly ITransient transient;

        public TransientExtensionsTests()
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
            var pdq = provider.GetService<IPdq>();
            this.transient = pdq.Begin();
        }

        [Theory]
        [MemberData(nameof(ExtensionTests))]
        public void ServicesShouldNotBeNull<T>(Func<ITransient, T> method)
        {
            // Act
            var result = method(this.transient);

            // Assert
            result.Should().NotBeNull();
        }

        public static IEnumerable<object[]> ExtensionTests
        {
            get
            {
                yield return new object[] { GetMethod(t => t.GetCommand<Person>()) };
                yield return new object[] { GetMethod(t => t.GetQuery<Person>()) };
                yield return new object[] { GetMethod(t => t.GetService<Person>()) };

                yield return new object[] { GetMethod(t => t.GetCommand<Person, int>()) };
                yield return new object[] { GetMethod(t => t.GetQuery<Person, int>()) };
                yield return new object[] { GetMethod(t => t.GetService<Person, int>()) };

                yield return new object[] { GetMethod(t => t.GetCommand<Address, int, int>()) };
                yield return new object[] { GetMethod(t => t.GetQuery<Address, int, int>()) };
                yield return new object[] { GetMethod(t => t.GetService<Address, int, int>()) };

                yield return new object[] { GetMethod(t => t.GetCommand<AddressNote, int, int, int>()) };
                yield return new object[] { GetMethod(t => t.GetQuery<AddressNote, int, int, int>()) };
                yield return new object[] { GetMethod(t => t.GetService<AddressNote, int, int, int>()) };

            }
        }

        private static Func<ITransient, T> GetMethod<T>(Expression<Func<ITransient, T>> expression)
            => expression.Compile();
    }
}

