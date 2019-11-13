using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class OrderBy_ : BaseClass
    {
        private StringBuilder sbText;

        internal string GetText()
        {
            if (sbText.ToString().Trim().Length <= 8)
                return "";
            return sbText.ToString();
        }

        public OrderBy_()
        {
            sbText = new StringBuilder();
            sbText.Append("ORDER BY");
        }

        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public OrderBy_ OrderBy(string columnName)
        {
            AddOrder(columnName,false);
            return this;
        }

        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public OrderBy_ OrderByDesc(string columnName)
        {
            AddOrder(columnName,true);
            return this;
        }

        internal void AddOrder(string columnName,bool desc)
        {
            if (sbText.ToString().Trim().Length <= 8)
                sbText.Append(" ");
            else
                sbText.Append(",");
            sbText.Append(columnName);
            if (desc)
                sbText.Append(" DESC");
        }


    }
}
