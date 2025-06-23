using System;
using System.Collections.Generic;
using decaf.common.Exceptions;
using FluentAssertions;
using Xunit;

namespace decaf.sqlite.tests;

public class ConnectionDetailsTests : SqliteTest
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
		string mode,
		bool readOnly,
		bool isNew)
	{
		// Arrange
		var connString = $"Data Source={path};Mode={mode};";

		// Act
		Func<ISqliteConnectionDetails> func = () => SqliteConnectionDetails.FromConnectionString(connString);

		// Assert
		func.Should().NotThrow<ConnectionStringParsingException>();

		var details = func();
		details.FullUri.Should().Be(path);
		details.ReadOnly.Should().Be(readOnly);
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
			yield return new object[] { ":memory:", "ReadWriteCreate", false, true };
			yield return new object[] { "C:\\MyDb.db", "ReadOnly", true, false };
			yield return new object[] { "/Users/myuser/Desktop/mydb.db", "ReadWriteCreate", false, true };
		}
	}
}