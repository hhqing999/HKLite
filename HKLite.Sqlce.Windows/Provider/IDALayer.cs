using System;

using System.Collections.Generic;
using System.Text;

using System.Data;

namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：IDALayer接口
     */
    public interface IDALayer : IDALayerBase
    {
        /// <summary>
        /// 通过SqlCeResultSet 高效复制 
        /// </summary>
        /// <param name="SqlCommand"></param>
        /// <param name="bulkCopy"></param>
        void BulkCopy(string SqlCommand, BulkCopyHandler bulkCopy);
    }
}
