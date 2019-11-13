using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class Where_ : BaseClass
    {
        private Condition myWhereRelation;
        private string customWhere;
        private OrderBy_ myOrderBy;
        private AndOr myAndOr;
        //private GroupBy_ myGroupBy;
        private WhereEntity myWhereEntity;

        internal Condition MyWhereRelation
        {
            get { return myWhereRelation; }
            set { myWhereRelation = value; }
        }

        internal AndOr MyAndOr
        {
            get { return myAndOr; }
            set { myAndOr = value; }
        }

        internal OrderBy_ MyOrderBy
        {
            get { return myOrderBy; }
            set { myOrderBy = value; }
        }

        //internal GroupBy_ MyGroupBy
        //{
        //    get { return myGroupBy; }
        //    set { myGroupBy = value; }
        //}

        internal string CustomWhere
        {
            get { return customWhere; }
        }

        internal WhereEntity MyWhereEntity
        {
            get { return myWhereEntity; }
            set { myWhereEntity = value; }
        }

        internal List<WhereEntity> ListWhereEntity = new List<WhereEntity>();

        public Where_()
        {
            myWhereRelation = new Condition();
            myWhereRelation.WhereParent = this;

            myAndOr = new AndOr();
            myAndOr.WhereParent = this;

            myOrderBy = new OrderBy_();   

            //myGroupBy = new GroupBy_();
            //myGroupBy.WhereParent = this;

            myWhereEntity = new WhereEntity();
            myWhereEntity.ConditionType = ConditionTypeEnum.None;
        }

      
        /// <summary>
        /// 自定义查询条件，如果需要，在参数中加where，如：where column1='a' and column2='b'
        /// </summary>
        /// <param name="customWhere"></param>
        /// <returns></returns>
        public void Custom(string customWhere)
        {
            this.customWhere = customWhere;
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Equal(string columnName, object value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.Equal, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 大于或等于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LargerOrEqual(string columnName, object value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.EqualOrLarger, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Larger(string columnName, object value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.Larger, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 小于或等于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition SmallerOrEqual(string columnName, object value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.EqualOrSmaller, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Smaller(string columnName, object value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.Smaller, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 等于null
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Condition IsNull(string columnName)
        {
            AddWhereEntity(columnName, RelationTypeEnum.IsNull, null);
            return myWhereRelation;
        }

        /// <summary>
        /// 模糊查询：like %xxx%
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition Like(string columnName, string value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.Like, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 模糊查询：like %xxx
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LikeLeft(string columnName, string value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.LikeLeft, value);
            return myWhereRelation;
        }

        /// <summary>
        /// 模糊查询：like xxx%
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LikeRight(string columnName, string value)
        {
            AddWhereEntity(columnName, RelationTypeEnum.LikeRight, value);
            return myWhereRelation;
        }
       

        internal void AddWhereEntity(string columnName, RelationTypeEnum relation, object value)
        {
            myWhereEntity.ColumnIndex = 0;
            myWhereEntity.ColumnName = columnName;
            myWhereEntity.Value = value;
            myWhereEntity.RelationType = relation;


            // 检查是否存在同列名，存在着将ColumnIndex在最大值的基础上加1
            int count =ListWhereEntity .Count ;
            if (count > 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    if (ListWhereEntity[i].ColumnName.Trim().ToLower() == columnName.Trim().ToLower())
                    {
                        myWhereEntity.ColumnIndex = ListWhereEntity[i].ColumnIndex + 1;
                        break;
                    }
                }
            }
            ListWhereEntity.Add(myWhereEntity);
        }


    }
}
