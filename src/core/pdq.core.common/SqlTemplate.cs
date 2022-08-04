using System;
namespace pdq.common
{
    public class SqlTemplate
    {
        public SqlTemplate(string sql, object parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public static SqlTemplate Create(string sql, object parameters)
            => new SqlTemplate(sql, parameters);

        public string Sql { get; }
        public object Parameters { get; }
    }
}

