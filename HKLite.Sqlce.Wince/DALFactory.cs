using System;

using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：创建DALayer工厂类
     */
    /// <summary>
    /// 创建DALayer工厂类
    /// </summary>
    public abstract class DALFactory
    {
        // 数据库名集合
        private static List<string> dbNames = new List<string>();

        /// <summary>
        /// 创建DALLayer 实例
        /// </summary>
        /// <param name="dbPath">数据库绝对路径</param>
        /// <param name="password">密码</param>
        /// <param name="entityDllName">实体程序集DLL名称</param>
        /// <param name="dbVersion">数据库版本号</param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        public static IDALayer CreateLayer(string dbPath, string password, string entityDllName, int dbVersion,bool longConnection)
        {
            return new DALayer(dbPath, password, entityDllName, dbVersion, longConnection, new DbProvider());
        }



    }
}
