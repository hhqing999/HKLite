using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：查询模块接口
     */
    public interface IQueryBuilder<T> : BaseInterface
    {
        /// <summary>
        /// 获取或设置自定义脚本
        /// </summary>
        string CustomSqlCommand { get; set; }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Select_ Select();

        /// <summary>
        /// 查询 前top条记录
        /// </summary>
        /// <param name="topCount">返回的记录数</param>
        /// <returns></returns>
        Select_ Select(int top);

        /// <summary>
        /// 查询 指定列
        /// </summary>
        /// <param name="selectColumns">查询列</param>
        /// <returns></returns>
        Select_ Select(params string[] columns);

        /// <summary>
        /// 查询 前top条记录，指定列
        /// </summary>
        /// <param name="topCount">返回的记录数</param>
        /// <param name="selectColumns">查询列</param>
        /// <returns></returns>
        Select_ Select(int top, params string[] columns);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <returns></returns>
        DataTable QueryTable();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        T QueryByKey(object keyValue);

        /// <summary>
        /// 获取一个实体
        /// </summary>
        /// <returns></returns>
        T QuerySingle();

        /// <summary>
        /// 根据条件获取实体集合
        /// </summary>
        /// <returns></returns>
        List<T> Query();

        /// <summary>
        /// 获取所有实体集合
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        List<T> QueryAll();

        /// <summary>
        /// 获取查询结果中的记录数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <returns></returns>
        object QueryAttribute();

        /// <summary>
        /// 当前要执行的Sql脚本集合
        /// </summary>
        List<string> SqlCommands { get; }

    }
}
