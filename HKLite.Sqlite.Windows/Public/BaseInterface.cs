using System;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace HKLite
{
    public interface BaseInterface
    {
        #region 隐藏代码
        [EditorBrowsable(EditorBrowsableState.Never)]
         bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();
        #endregion 
    }
}
