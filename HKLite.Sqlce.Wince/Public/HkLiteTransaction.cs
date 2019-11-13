using System;

using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace HKLite
{
    public class HKLiteTransaction
    {
        private DbTransaction trans;
        private bool longConnection;
        internal HKLiteTransaction(DbTransaction trans, bool longConnection)
        {
            this.trans = trans;
            this.longConnection = longConnection;
        }

        internal DbTransaction DbTransaction
        {
            get
            {
                return trans;
            }
        }

        /// <summary>
        /// 提交事务。提交完成后，若不是长连接，则销毁当前连接
        /// </summary>
        public void Commit()
        {
            if (trans == null)
                return;

            var conntion = trans.Connection;
            trans.Commit();
            trans.Dispose();
            if (!longConnection)
            {              
                if (conntion.State != ConnectionState.Closed)
                    conntion.Close();
                conntion.Dispose();               
            }
        }

        /// <summary>
        /// 回滚事务。回滚完成后，若不是长连接，则销毁当前连接
        /// </summary>
        public void Rollback()
        {
            if (trans == null)
                return;

            var conntion = trans.Connection;
            trans.Rollback();
            trans.Dispose();
            if (!longConnection)
            {               
                if (conntion.State != ConnectionState.Closed)
                    conntion.Close();
                conntion.Dispose();              
            }
        }
    }
}
