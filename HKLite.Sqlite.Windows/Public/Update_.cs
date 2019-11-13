using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class Update_ : BaseClass
    {
        private Set_ mySet;

        internal Set_ MySet
        {
            get
            {
                if (mySet == null)
                    mySet = new Set_();
                return mySet;
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
            mySet.DitUpdateInfo.Add(columnName, value);
            return mySet;
        }

       
    }
}
