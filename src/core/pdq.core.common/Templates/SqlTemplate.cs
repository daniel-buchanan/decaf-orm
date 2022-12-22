using System;
using System.Collections.Generic;

namespace pdq.common.Templates
{
    public class SqlTemplate
    {
        public SqlTemplate(string sql, IEnumerable<SqlParameter> parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public static SqlTemplate Create(string sql, IEnumerable<SqlParameter> parameters)
            => new SqlTemplate(sql, parameters);

        /// <summary>
        /// 
        /// </summary>
        public string Sql { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SqlParameter> Parameters { get; }
    }
}

