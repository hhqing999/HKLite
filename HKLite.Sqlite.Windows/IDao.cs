using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;


namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2015-5-18
     * 描述：增删查改模块接口
     */
    public interface IDao<T> : BaseInterface
    {
        IQueryBuilder<T> QueryBuilder();

        [EditorBrowsable(EditorBrowsableState.Never)]
        IInsertBuilder<T> InsertBuilder();

        IUpdateBuilder<T> UpdateBuilder();

        IDeleteBuilder<T> DeleteBuilder();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        T QueryByKey(object keyValue);

        /// <summary>
        /// 获取所有对象集合
        /// </summary>
        /// <returns></returns>
        List<T> QueryAll();

        /// <summary>
        /// 插入，返回插入的记录数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        /// 根据主键更新，返回更新的记录数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateByKey(T entity);

        /// <summary>
        /// 根据主键删除，返回删除的记录数
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        int DeleteByKey(object keyValue);

        /// <summary>
        /// 删除所有记录，返回删除的记录数
        /// </summary>
        /// <returns></returns>
        int DeleteAll();
    }
}
