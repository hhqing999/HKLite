using System;

using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2015-09-29
     * 描述：创建DALayer工厂类
     */
    /// <summary>
    /// 创建DALayer工厂类
    /// </summary>
    public abstract class DALFactory
    {
        /// <summary>
        /// 创建DALLayer 实例
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="entityDllName">实体集名称</param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        public static IDALayer CreateLayer(string connectionString, string entityDllName, bool longConnection)
        {
            string dbName = entityDllName.Replace(".", "_").ToLower();           
            return new DALayer(connectionString, entityDllName, longConnection, new DbProvider());
        }

        /// <summary>
        /// 测试连接字符串能否连接到数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool TestConnection(string connectionString, ref string msg)
        {
            if (string.IsNullOrEmpty(connectionString.Trim()))
            {
                msg = "连接字符串为空";
                return false;
            }
            try
            {
                IDbProvider iprovider = new DbProvider();
                object obj = SqlHelper.ExecuteScalar(iprovider.CreateConnection(connectionString), false, "select 1");
                if (obj == null || obj == DBNull.Value)
                    return false;
                if (Convert.ToInt32(obj) == 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

    }
}
