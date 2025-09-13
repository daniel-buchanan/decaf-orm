using System;
using decaf.db.common;
using Xunit;

namespace decaf.npgsql.tests;

public class NpgsqlTypeParserTests
{
    private readonly ITypeParser _parser = new NpgsqlTypeParser();

    [Theory]
    [MemberData(nameof(ParseData))]
    public void ParseSuccess(Type input, string expected)
    {
        // Act
        var result = _parser.Parse(input);
        
        // Assert
        Assert.Equal(expected, result);
    }

    public static TheoryData<Type, string> ParseData => new()
    {
        { typeof(int), "integer" },
        { typeof(long), "bigint" },
        { typeof(float), "float" },
        { typeof(double), "float" },
        { typeof(decimal), "float" },
        { typeof(Single), "float" },
        { typeof(string), "text" },
        { typeof(bool), "boolean" },
        { typeof(byte[]), "bytea" },
        { typeof(DateTime), "timestamp" },
        { typeof(DateTimeOffset), "timestampz" },
        { typeof(TimeOnly), "time" },
        { typeof(DateOnly), "date" },
        { typeof(Guid), "uuid" },
        { typeof(Exception), "text" }
    };
}