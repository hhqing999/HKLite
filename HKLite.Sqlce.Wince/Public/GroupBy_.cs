using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class GroupBy_ : BaseClass
    {
        private string groupByText="";

        private Where_ whereParent;

        internal Where_ WhereParent
        {
            get { return whereParent; }
            set { whereParent = value; }
        }

        internal string GroupByText
        {
            get { return groupByText; }
            set { groupByText = value; }
        }

        internal void SetGroupBy(params string[] columns)
        {
            groupByText = " GROUP BY ";
            if (columns != null && columns.Length > 0)
            {
                int n = 0;
                foreach (string item in columns)
                {
                    if (item != null && item.Trim().Length > 0)
                    {
                        if (n == 0)
                            groupByText = string.Format("{0} {1}", groupByText, item);
                        else
                            groupByText = string.Format("{0},{1}", groupByText, item);
                        n++;
                    }
                }
            }
        }

        internal string GetText()
        {
            if (groupByText.Trim().Length <= 8)
                return "";
            return groupByText;
        }

        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public OrderBy_ OrderBy(string columnName)
        {
            whereParent.MyOrderBy.AddOrder(columnName, false);
            return whereParent.MyOrderBy;
        }

        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public OrderBy_ OrderByDesc(string columnName)
        {
            whereParent.MyOrderBy.AddOrder(columnName, true);
            return whereParent.MyOrderBy;
        }




    }
}
