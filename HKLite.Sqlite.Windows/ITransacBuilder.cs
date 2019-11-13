using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：事务模块接口
     */
    public interface ITransacBuilder : BaseInterface
    {
        IDao<T> Dao<T>();

        /// <summary>
        /// 获取QueryBuilder实例，此方法与IDALayer中的QueryBuilder()方法效果一样
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
        /// 添加自定义脚本
        /// </summary>
        /// <param name="cmdText"></param>
        void AddCommandText(string cmdText);

        void AddCommandText(CommandType commandType, string cmdText, params DbParameter[] param); 

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <returns></returns>
        bool Run();


    }
}
