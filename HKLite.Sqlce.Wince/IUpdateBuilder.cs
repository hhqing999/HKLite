using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：更新模块接口
     */
    public interface IUpdateBuilder<T> : BaseInterface
    {
        /// <summary>
        /// 更新表
        /// </summary>
        /// <returns></returns>
        Update_ Update();
        
        /// <summary>
        /// 根据主键更新表，entity中需要更新的值不等于null,否则为null。主键对应的属性值不能为null，不更新主键
        /// </summary>
        /// <param name="entity">更新实体</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void UpdateByKey(T entity);

        /// <summary>
        /// 根据主键更新表，entity中需要更新的值不等于null,否则为null。主键对应的属性值不能为null，不更新主键
        /// </summary>
        /// <param name="entity">更新实体</param>
        /// <param name="excludeColumns">不更新的字段</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void UpdateByKey(T entity, params string[] excludeColumns);

        /// <summary>
        /// 执行更新操作。若使用事务，则应执行ITransacBuilder中的Run()方法，否则报错
        /// </summary>
        /// <returns>返回更新的记录数</returns>
        int Run();

        /// <summary>
        /// 当前要执行的Sql脚本集合
        /// </summary>
        List<string> SqlCommands { get; }
    }
}
