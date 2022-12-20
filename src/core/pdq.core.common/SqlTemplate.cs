using System;
using System.Collections.Generic;

namespace pdq.common
{
    public class SqlTemplate
    {
        public SqlTemplate(string sql, IEnumerable<string> parameterNames)
        {
            Sql = sql;
            ParameterNames = parameterNames;
        }

        public static SqlTemplate Create(string sql, IEnumerable<string> parameterNames)
            => new SqlTemplate(sql, parameterNames);

        /// <summary>
        /// 
        /// </summary>
        public string Sql { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ParameterNames { get; }
    }
}

