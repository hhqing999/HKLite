using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace HKLite
{
    internal interface IDbProvider
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        DbConnection CreateConnection();

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        DbConnection CreateConnection(string connectionString);

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string GetConnectionString(string dbPath, string password);

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns></returns>
        DbCommand CreateCommand();

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string paramName, object paramValue);

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <returns></returns>
        DbParameter CreateParameter();

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <returns></returns>
        DbDataAdapter CreateDataAdapter();

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="password"></param>
        void CreateDatabase(string dbPath, string password);
               
        /// <summary>
        /// 获取数据库表脚本
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        string GetTableScript(Type entityType);

        /// <summary>
        ///  获取表中新增列的sql脚本，如果该表不存在，则返回创建表的脚本
        /// </summary>
        /// <param name="dalInfo"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        List<string> GetNewColumnsCommandTexts(DalLayerInfo dalInfo, Type entityType);

        /// <summary>
        /// 获取查询脚本
        /// </summary>
        /// <param name="top"></param>
        /// <param name="columns"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        string GetQueryText(int top, string[] columns, string tableName, string where);
    }
}
