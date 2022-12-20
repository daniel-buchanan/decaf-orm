using System.Text;

namespace pdq.db.common.Builders
{
	public sealed class SqlBuilder : ISqlBuilder
	{
        const string Indent = "  ";
        const string Crlf = "\r\n";
        const string Lf = "\n";

        private int indentLevel = 0;
        private string lineFeedChar;
        private readonly StringBuilder stringBuilder;

        public SqlBuilder()
		{
            this.stringBuilder = new StringBuilder();
            this.lineFeedChar = Crlf;
		}

        /// <inheritdoc/>
        public string GetSql()
            => this.stringBuilder.ToString();

        /// <inheritdoc/>
        public string LineEnding => this.lineFeedChar;

        public void IncreaseIndent()
            => this.indentLevel = this.indentLevel + 1;

        public void DecreaseIndent()
        {
            if (this.indentLevel == 0) return;
            this.indentLevel = this.indentLevel - 1;
        }

        public void UseCrlf()
            => this.lineFeedChar = Crlf;

        public void UseLf()
            => this.lineFeedChar = Lf;

        public void Append(string value)
            => this.stringBuilder.Append(value);

        public void Append(string formatStr, params object[] parameters)
            => this.stringBuilder.AppendFormat(formatStr, parameters);

        public void AppendLine()
            => this.stringBuilder.Append(this.lineFeedChar);

        public void AppendLine(string value)
        {
            this.PrependIndent();
            this.stringBuilder.AppendFormat("{0}{1}", value, this.lineFeedChar);
        }

        public void AppendLine(string formatStr, params object[] parameters)
        {
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
            for(var i = 0; i < this.indentLevel; i++)
            {
                this.stringBuilder.Append(Indent);
            }
        }
    }
}

