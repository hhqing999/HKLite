using System;

using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class EntityAttribute : Attribute
    {
        private bool identity = false;
        private bool primaryKey = false;
        private object length;
        private string dataType = "";
        //private bool arrowNull = true;
        private bool displayOnly = false;

        public EntityAttribute()
        {
            Assembly assem = Assembly.GetCallingAssembly(); 
        }

        /// <summary>
        /// 是否自增字段
        /// </summary>
        public bool Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }

        }

        /// <summary>
        /// 字段长度 该属性仅对String和Decimal类型有效。String默认长度：50，Decimal默认长度：9,1。
        /// </summary>
        public object Length
        {
            get{return length ;}
            set { length = value; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        ///// <summary>
        ///// 是否允许为null
        ///// </summary>
        //public bool ArrowNull
        //{
        //    get { return arrowNull; }
        //    set { arrowNull = value; }
        //}

        /// <summary>
        /// 是否只用于显示，true-不作为表字段;false-作为表字段
        /// </summary>
        public bool DisplayOnly
        {
            get { return displayOnly; }
            set { displayOnly = value; }
        }

    }
}
