using System;
namespace pdq.db.common.Builders
{
	public interface ISqlBuilder : IGetSql
	{
		string LineEnding { get; }

		void IncreaseIndent();

		void DecreaseIndent();

		void PrependIndent();

        void UseCrlf();

		void UseLf();

		void Append(string value);

		void Append(string formatStr, params object[] parameters);

		void AppendLine();

		void AppendLine(string value);

		void AppendLine(string formatStr, params object[] parameters);
	}
}

