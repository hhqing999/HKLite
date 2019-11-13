using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class Condition : BaseClass
    {
        private Where_ whereParent;
     
        internal Where_ WhereParent
        {
            get { return this.whereParent; }
            set { whereParent = value; }
        }

        /// <summary>
        /// and
        /// </summary>
        /// <returns></returns>
        public AndOr And()
        {
            whereParent.MyWhereEntity = new WhereEntity();
            whereParent.MyWhereEntity.ConditionType = ConditionTypeEnum.And;

            return whereParent.MyAndOr;
        }

        /// <summary>
        /// or
        /// </summary>
        /// <returns></returns>
        public AndOr Or()
        {
            whereParent.MyWhereEntity = new WhereEntity();
            whereParent.MyWhereEntity.ConditionType = ConditionTypeEnum.Or;

            return whereParent.MyAndOr;
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

        ///// <summary>
        ///// 分组
        ///// </summary>
        ///// <param name="columns"></param>
        ///// <returns></returns>
        //public GroupBy_ GroupBy(params string[] columns)
        //{
        //    whereParent.MyGroupBy.SetGroupBy(columns);
        //    return whereParent.MyGroupBy;
        //}


    }
}
