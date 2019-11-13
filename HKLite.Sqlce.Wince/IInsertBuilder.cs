using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：插入模块接口
     */
    public interface IInsertBuilder<T> : BaseInterface
    {
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="excludeColumns">排除列</param>
        void Insert(T entity, params string[] excludeColumns);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Insert(T entity);

        /// <summary>
        /// 执行插入操作，若用事务，则应执行Itransacuilder中的Run()方法，否则报错
        /// </summary>
        /// <returns>返回插入的记录数</returns>
        int Run();

        /// <summary>
        /// 当前要执行的Sql脚本集合
        /// </summary>
        List<string> SqlCommands { get; }

    }
}
