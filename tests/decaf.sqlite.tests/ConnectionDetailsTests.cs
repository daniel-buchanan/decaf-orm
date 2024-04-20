using System;
using System.Collections.Generic;
using decaf.common.Connections;
using decaf.common.Exceptions;
using FluentAssertions;
using Xunit;

namespace decaf.sqlite.tests
{
	public class ConnectionDetailstests : SqliteTest
	{
		[Theory]
		[MemberData(nameof(InvalidConnectionStrings))]
		public void InvalidConnectionStringFails(string connectionString)
		{
			// Act
			Action method = () => SqliteConnectionDetails.FromConnectionString(connectionString);

			// Assert
			method.Should().Throw<ConnectionStringParsingException>();
		}

		[Theory]
		[MemberData(nameof(ValidConnectionStrings))]
		public void ValidConnectionStringPasses(
			string path,
			int version,
			bool isNew)
		{
			// Arrange
			var connString = $"Data Source={path};Version={version};New={isNew};";

			// Act
			Func<ISqliteConnectionDetails> func = () => SqliteConnectionDetails.FromConnectionString(connString);

			// Assert
			func.Should().NotThrow<ConnectionStringParsingException>();

			var details = func();
			details.FullUri.Should().Be(path);
			details.Version.Should().Be(version);
			details.CreateNew.Should().Be(isNew);
		}

		public static IEnumerable<object[]> InvalidConnectionStrings
		{
			get
			{
                yield return new object[] { null };
                yield return new object[] { string.Empty };
				yield return new object[] { "Bob=Hello;" };
				yield return new object[] { "Host=bob;" };
                yield return new object[] { "Host=bob;Port=5432;" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgres;" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgres;Username=admin;" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgres;Username=admin" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgresUsername=admin;" };
            }
		}

		public static IEnumerable<object[]> ValidConnectionStrings
		{
			get
			{
				yield return new object[] { ":memory:", 1, true };
                yield return new object[] { "C:\\MyDb.db", 2, false };
                yield return new object[] { "/Users/myuser/Desktop/mydb.db", 3, true };
            }
		}
	}
}

