using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class Select_ : BaseClass
    {
        private Where_ myWhere;
   
        internal Where_ MyWhere
        {
            get { return myWhere; }
        }

        internal string WhereText
        {
            get
            {
                return string.Format("{0} {1}", Common.GetWhereText(myWhere), Common.GetOrderText(myWhere.MyOrderBy));
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
        /// 查询
        /// </summary>
        public Select_()
        {
            if (myWhere == null)
                myWhere = new Where_();
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public Where_ Where()
        {
            return myWhere;
        }

        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public OrderBy_ OrderBy(string columnName)
        {
            this.myWhere.MyOrderBy.AddOrder(columnName, false);
            return this.myWhere.MyOrderBy;
        }

        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public OrderBy_ OrderByDesc(string columnName)
        {
            this.myWhere.MyOrderBy.AddOrder(columnName, true);
            return this.myWhere.MyOrderBy;
        }

        ///// <summary>
        ///// 分组
        ///// </summary>
        ///// <param name="columns"></param>
        ///// <returns></returns>
        //public GroupBy_ GroupBy(params string[] columns)
        //{
        //    myWhere.MyGroupBy.SetGroupBy(columns);
        //    return myWhere.MyGroupBy;
        //}


    }
}
