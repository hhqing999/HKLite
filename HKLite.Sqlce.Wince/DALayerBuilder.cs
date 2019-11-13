using System;

using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：创建DALayer类
     */
    /// <summary>
    /// 创建DALayer类
    /// </summary>
    public class DALayerBuilder
    {
        /// <summary>
        /// 创建DALayer 实例
        /// </summary>
        /// <param name="dbPath">数据库绝对路径</param>
        /// <param name="password">密码</param>
        /// <param name="entityDllName">实体程序集DLL名称</param>
        /// <param name="dbVersion">数据库版本号</param>
        /// <param name="isLongConnection">是否长连接</param>
        /// <returns></returns>
        public static IDALayer CreateLayer(string dbPath, string password, string entityDllName, int dbVersion,bool isLongConnection)
        {
            return new DALayer(dbPath, password, entityDllName, dbVersion, isLongConnection, new DbProvider());
        }



    }
}
