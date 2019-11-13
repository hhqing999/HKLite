using System;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace HKLite
{
    public class BaseClass
    {
        #region 隐藏代码
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
        #endregion 
    }
}
