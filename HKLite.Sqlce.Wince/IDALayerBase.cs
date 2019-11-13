using System;

using System.Collections.Generic;
using System.Text;

using System.Data;
using System.ComponentModel;
using System.Data.Common;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：IDALayer接口
     */
    public interface IDALayerBase : BaseInterface
    {
        /// <summary>
        /// 数据库创建时触发的事件
        /// </summary>
        OnCreatedDatabase CreatedDatabase { get; set; }

        /// <summary>
        /// 数据库升级时触发的事件
        /// </summary>
        OnUpdatedDataBase UpgradedDataBase { get; set; }

        /// <summary>
        /// 获取数据库路径
        /// </summary>
        string DatabasePath { get; }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        bool Init();

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
        /// 执行Sql脚本
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        object ExecuteScalar(string cmdText);

        /// <summary>
        /// 在事务执行Sql脚本
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        bool ExecuteTransac(params string[] cmdText);

        /// <summary>
        /// 在事务中执行Sql脚本
        /// </summary>
        /// <param name="cmdTexts"></param>
        /// <returns></returns>
        bool ExecuteTransac(List<string> cmdTexts);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        DataTable ExecuteTable(string cmdText);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbReader"></param>
        void ExecuteReader(string sqlCommand, ReaderDelegateHandler dbReader);

        
    }
}
