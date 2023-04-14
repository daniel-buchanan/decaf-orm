using System.Collections.Generic;
using pdq.common;
using pdq.common.Connections;

namespace pdq.sqlserver
{
    public interface ISqlServerConnectionDetails : IConnectionDetails
    {
        /// <summary>
        /// 
        /// </summary>
        void IsTrustedConnection();
    }
}