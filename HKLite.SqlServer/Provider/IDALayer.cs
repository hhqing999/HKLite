using System;

using System.Collections.Generic;
using System.Text;

using System.Data;
using System.ComponentModel;
using System.Data.Common;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2015-09-30
     * 描述：IDALayer接口
     */
    public interface IDALayer
    {
        ///// <summary>
        ///// 数据库创建时触发的事件
        ///// </summary>
        //OnCreatedDatabase CreatedDatabase { get; set; }

        ///// <summary>
        ///// 数据库升级时触发的事件
        ///// </summary>
        //OnUpdatedDataBase UpgradedDataBase { get; set; }

        ///// <summary>
        ///// 获取数据库路径
        ///// </summary>
        //string DatabasePath { get; }

        /// <summary>
        /// 创建表 只有初始化后才能调用该方法
        /// </summary>
        /// <returns></returns>
        bool CreateTables();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        bool Init(ref string msg);

        /// <summary>
        /// 打开数据库连接，仅对长连接有效
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库连接，仅对长连接有效
        /// </summary>
        void Close();

        /// <summary>
        /// 将实体类结构同步到数据库，只进行新增表和新增列操作
        /// </summary>
        /// <param name="transacBuilder">进行数据结构同步操作的事务，需手动调用TransacBuilder的Run()方法</param>
        void SyncTables(ITransacBuilder transacBuilder);

        IDao<T> Dao<T>();

        /// <summary>
        /// 获取QueryBuilder实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryBuilder<T> QueryBuilder<T>();

        /// <summary>
        /// 获取InsertBuilder实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IInsertBuilder<T> InsertBuilder<T>();

        /// <summary>
        /// 获取UpdateBuilder实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IUpdateBuilder<T> UpdateBuilder<T>();

        /// <summary>
        /// 获取DeleteBuilder实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDeleteBuilder<T> DeleteBuilder<T>();

        /// <summary>
        /// 获取TransacBuilder实例
        /// </summary>
        /// <returns></returns>
        ITransacBuilder TransacBuilder();

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        HKLiteTransaction GetTransaction();

        /// <summary>
        /// 执行Sql脚本
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText);

        int ExecuteNonQuery(CommandType commandType, string cmdText);

        int ExecuteNonQuery(CommandType commandType, string cmdText, params DbParameter[] param);

        /// <summary>
        /// 使用事务执行Sql脚本
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        int ExecuteNonQuery(HKLiteTransaction trans, string cmdText);

        /// <summary>
        /// 使用事务执行Sql脚本
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteNonQuery(HKLiteTransaction trans, string cmdText, params DbParameter[] param);

        /// <summary>
        /// 使用事务执行Sql脚本
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteNonQuery(HKLiteTransaction trans, CommandType commandType, string cmdText, params DbParameter[] param);
        
        /// <summary>
        /// 执行Sql脚本
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        object ExecuteScalar(string cmdText);

        object ExecuteScalar(CommandType commandType, string cmdText);

        object ExecuteScalar(CommandType commandType, string cmdText, params DbParameter[] param);

        /// <summary>
        /// 在事务执行Sql脚本
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        bool ExecuteTransac(params string[] cmdText);

        bool ExecuteTransac(CommandType[] commandType, params string[] cmdText);

        bool ExecuteTransac(CommandType[] commandType, string[] cmdText, DbParameter[][] param);

        /// <summary>
        /// 在事务中执行Sql脚本
        /// </summary>
        /// <param name="cmdTexts"></param>
        /// <returns></returns>
        bool ExecuteTransac(List<string> cmdTexts);

        bool ExecuteTransac(List<CommandType> commandType, List<string> cmdTexts);

        bool ExecuteTransac(List<CommandType> commandType, List<string> cmdTexts, List<DbParameter[]> param);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        DataTable ExecuteTable(string cmdText);

        DataTable ExecuteTable(CommandType commandType, string cmdText);

        DataTable ExecuteTable(CommandType commandType, string cmdText, params DbParameter[] param);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbReader"></param>
        void ExecuteReader(string sqlCommand, ReaderDelegateHandler dbReader);

        void ExecuteReader(CommandType commandType, string sqlCommand, ReaderDelegateHandler dbReader);

        void ExecuteReader(CommandType commandType, string sqlCommand, ReaderDelegateHandler dbReader, params DbParameter[] param);
    }
}
