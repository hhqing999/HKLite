using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace HKLite
{
    /// <summary>
    /// 数据读取代理
    /// </summary>
    /// <param name="reader">数据读取器</param>
    public delegate void ReaderDelegateHandler(DbDataReader reader);

    /// <summary>
    /// 创建数据库事件代理
    /// </summary>
    public delegate void OnCreatedDatabase();

    /// <summary>
    /// 升级数据库事件代理
    /// </summary>
    /// <param name="oldVersion">旧版本号</param>
    /// <param name="newVersion">新版本号</param>
    public delegate void OnUpdatedDataBase(int oldVersion,int newVersion);
}
