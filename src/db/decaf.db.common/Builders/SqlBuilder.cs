using System;
using System.Text;

namespace decaf.db.common.Builders;

public sealed class SqlBuilder : ISqlBuilder
{
    public const string Indent = "  ";

    private int indentLevel = 0;
    private readonly string lineFeedChar;
    private readonly StringBuilder stringBuilder;
    private readonly bool noOp;

    private SqlBuilder(bool noOp)
    {
        stringBuilder = new StringBuilder();
        lineFeedChar = Environment.NewLine;
        this.noOp = noOp;
    }

    public static ISqlBuilder Create() => new SqlBuilder(false);

    public static ISqlBuilder CreateNoOp() => new SqlBuilder(true);

    /// <inheritdoc/>
    public string GetSql()
        => stringBuilder.ToString();

    /// <inheritdoc/>
    public string LineEnding => lineFeedChar;

    public int IncreaseIndent()
    {
        if (noOp) return indentLevel;
        indentLevel = indentLevel + 1;
        return indentLevel;
    }

    public int DecreaseIndent()
    {
        if (noOp) return indentLevel;
        if (indentLevel == 0) return indentLevel;
        indentLevel = indentLevel - 1;
        return indentLevel;
    }

    public void Append(string value)
    {
        if (noOp) return;
        stringBuilder.Append(value);
    }

    public void Append(string formatStr, params object[] parameters)
    {
        if (noOp) return;
        stringBuilder.AppendFormat(formatStr, parameters);
    }

    public void AppendLine()
    {
        if (noOp) return;
        stringBuilder.Append(lineFeedChar);
    }

    public void AppendLine(string value)
    {
        if (noOp) return;
        PrependIndent();
        stringBuilder.AppendFormat("{0}{1}", value, lineFeedChar);
    }

    public void AppendLine(string formatStr, params object[] parameters)
    {
        if (noOp) return;
        PrependIndent();
        var newLength = parameters.Length + 1;
        var newEnd = parameters.Length;
        var newParameters = new object[newLength];
        for (var i = 0; i < parameters.Length; i++)
            newParameters[i] = parameters[i];

        formatStr += $"{{{newEnd}}}";
        newParameters[newEnd] = lineFeedChar;
        stringBuilder.AppendFormat(formatStr, newParameters);
    }

    public void PrependIndent()
    {
        if (noOp) return;
        for(var i = 0; i < indentLevel; i++)
        {
            stringBuilder.Append(Indent);
        }
    }
}