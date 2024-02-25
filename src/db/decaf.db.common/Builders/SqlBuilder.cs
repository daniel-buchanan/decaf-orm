﻿using System;
using System.Text;

namespace decaf.db.common.Builders
{
	public sealed class SqlBuilder : ISqlBuilder
	{
        public const string Indent = "  ";

        private int indentLevel = 0;
        private readonly string lineFeedChar;
        private readonly StringBuilder stringBuilder;
        private readonly bool noOp;

        private SqlBuilder(bool noOp)
		{
            this.stringBuilder = new StringBuilder();
            this.lineFeedChar = Environment.NewLine;
            this.noOp = noOp;
		}

        public static ISqlBuilder Create() => new SqlBuilder(false);

        public static ISqlBuilder CreateNoOp() => new SqlBuilder(true);

        /// <inheritdoc/>
        public string GetSql()
            => this.stringBuilder.ToString();

        /// <inheritdoc/>
        public string LineEnding => this.lineFeedChar;

        public int IncreaseIndent()
        {
            if (noOp) return this.indentLevel;
            this.indentLevel = this.indentLevel + 1;
            return this.indentLevel;
        }

        public int DecreaseIndent()
        {
            if (noOp) return this.indentLevel;
            if (this.indentLevel == 0) return this.indentLevel;
            this.indentLevel = this.indentLevel - 1;
            return this.indentLevel;
        }

        public void Append(string value)
        {
            if (noOp) return;
            this.stringBuilder.Append(value);
        }

        public void Append(string formatStr, params object[] parameters)
        {
            if (noOp) return;
            this.stringBuilder.AppendFormat(formatStr, parameters);
        }

        public void AppendLine()
        {
            if (noOp) return;
            this.stringBuilder.Append(this.lineFeedChar);
        }

        public void AppendLine(string value)
        {
            if (noOp) return;
            this.PrependIndent();
            this.stringBuilder.AppendFormat("{0}{1}", value, this.lineFeedChar);
        }

        public void AppendLine(string formatStr, params object[] parameters)
        {
            if (noOp) return;
            this.PrependIndent();
            var newLength = parameters.Length + 1;
            var newEnd = parameters.Length;
            var newParameters = new object[newLength];
            for (var i = 0; i < parameters.Length; i++)
                newParameters[i] = parameters[i];

            formatStr += $"{{{newEnd}}}";
            newParameters[newEnd] = this.lineFeedChar;
            this.stringBuilder.AppendFormat(formatStr, newParameters);
        }

        public void PrependIndent()
        {
            if (noOp) return;
            for(var i = 0; i < this.indentLevel; i++)
            {
                this.stringBuilder.Append(Indent);
            }
        }
    }
}

