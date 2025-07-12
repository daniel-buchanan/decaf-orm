namespace decaf.common.Utilities;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string? str)
        => string.IsNullOrWhiteSpace(str);
}