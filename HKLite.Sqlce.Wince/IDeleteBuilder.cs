using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
namespace HKLite
{
    /* 作者：何宏卿
     * 日期：2014-12-23
     * 描述：删除模块接口
     */
    public interface IDeleteBuilder<T> : BaseInterface
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Delete_ Delete();

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void DeleteByKey(object keyValue);

        /// <summary>
        /// 执行删除操作，若用事务，则必须执行Itransacuilder中的Run()方法
        /// </summary>
        /// <returns>返回删除的记录数</returns>
        int Run();

        /// <summary>
        /// 当前要执行的Sql脚本集合
        /// </summary>
        List<string> SqlCommands { get; }

    }
}
