using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class AndOr:BaseClass
    {
        private Where_ whereParent;
      
        internal Where_ WhereParent
        {
            get { return whereParent; }
            set { whereParent = value; }
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Equal(string columnName, object value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.Equal, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 大于或等于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LargerOrEqual(string columnName, object value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.EqualOrLarger, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Larger(string columnName, object value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.Larger, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 小于或等于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition SmallerOrEqual(string columnName, object value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.EqualOrSmaller, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Smaller(string columnName, object value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.Smaller, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 为null
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Condition IsNull(string columnName)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.IsNull, null);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 模糊查询：like %xxx%
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Like(string columnName, string value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.Like, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 模糊查询：like %xxx
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LikeLeft(string columnName, string value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.LikeLeft, value);
            return whereParent.MyWhereRelation;
        }

        /// <summary>
        /// 模糊查询：like xxx%
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LikeRight(string columnName, string value)
        {
            whereParent.AddWhereEntity(columnName, RelationTypeEnum.LikeRight, value);
            return whereParent.MyWhereRelation;
        }


    }
}
