using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    /// <summary>
    /// 批量复制代理
    /// </summary>
    /// <param name="rsSet"></param>
    /// <param name="rd"></param>
    public delegate void BulkCopyHandler(System.Data.SqlServerCe.SqlCeResultSet rsSet, System.Data.SqlServerCe.SqlCeUpdatableRecord rd);
}
