using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    public class Delete_ : BaseClass
    {
        private Where_ myWhere;

        internal Where_ MyWhere
        {
            get { return myWhere; }
        }

        /// <summary>
        /// 删除条件
        /// </summary>
        /// <returns></returns>
        public Where_ Where()
        {
            if (myWhere == null)
                myWhere = new Where_();
            return myWhere;
        }



    }
}
