using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace decaf.sqlite.tests;

public class SqliteTypeParserTests
{
    [Theory]
    [MemberData(nameof(TestCases))]
    public void CheckValueParses(Type input, string expected)
    {
        // Arrange
        var typeParser = new SqliteTypeParser();

        // Act
        var result = typeParser.Parse(input);

        // Assert
        result.Should().Be(expected);
    }
    
    public static IEnumerable<object[]> TestCases
    {
        get
        {
            yield return [typeof(string), "text"];
            yield return [typeof(String), "text"];
            yield return [typeof(int), "integer"];
            yield return [typeof(Int32), "integer"];
            yield return [typeof(decimal), "float"];
            yield return [typeof(float), "float"];
            yield return [typeof(double), "float"];
            yield return [typeof(Single), "float"];
            yield return [typeof(bool), "boolean"];
            yield return [typeof(Boolean), "boolean"];
            yield return [typeof(byte), "blob"];
            yield return [typeof(DateTime), "timestamp"];
            yield return [typeof(DateTimeOffset), "timestamp"];
            yield return [null, "text"];
            yield return [typeof(char), "text"];
            yield return [typeof(long), "integer"];
            yield return [typeof(Int64), "integer"];
            yield return [typeof(object), "text"];
        }
    }
}