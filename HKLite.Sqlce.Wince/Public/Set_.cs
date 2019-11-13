using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class Set_ : BaseClass
    {
        private Where_ myWhere;

        /// <summary>
        /// 更新的属性和值集合
        /// </summary>
        internal Dictionary<string, object> DitUpdateInfo = new Dictionary<string, object>();
        internal Where_ MyWhere
        {
            get
            {
                if (myWhere == null)
                    myWhere = new Where_();
                return myWhere;
            }
        }

        internal List<WhereEntity> ListWhereEntity
        {
            get
            {
                if (myWhere == null)
                    return null;             
                return myWhere.ListWhereEntity;
            }
        }

        /// <summary>
        /// 设置更新字段
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Set_ Set(string columnName, object value)
        {
            this.DitUpdateInfo.Add(columnName, value);
            return this;
        }

        /// <summary>
        /// 更新条件
        /// </summary>
        /// <returns></returns>
        public Where_ Where()
        {
            return myWhere;
        }


    }
}
