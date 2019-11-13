using System;

using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Common;

namespace HKLite
{
    internal abstract class BulkCopyService
    {
       // internal abstract DbConnection GetConnection();

        internal abstract DalLayerInfo GetDalyerInfo();

        public void BulkCopy(string SqlCommand, BulkCopyHandler bulkCopy)
        {
            DalLayerInfo dalInfo = GetDalyerInfo();
            DbConnection conntion = dalInfo.Connection;
            if (!dalInfo.LongConnection)
                conntion.Open();
            using (System.Data.SqlServerCe.SqlCeCommand comm = (System.Data.SqlServerCe.SqlCeCommand)conntion.CreateCommand())
            {
                comm.CommandText = SqlCommand;
                comm.CommandType = CommandType.Text;
                System.Data.SqlServerCe.SqlCeResultSet ceRsSet = comm.ExecuteResultSet(System.Data.SqlServerCe.ResultSetOptions.Scrollable | System.Data.SqlServerCe.ResultSetOptions.Updatable);

                try
                {
                    System.Data.SqlServerCe.SqlCeUpdatableRecord upRs = ceRsSet.CreateRecord();
                    if (bulkCopy != null)
                        bulkCopy(ceRsSet, upRs);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ceRsSet.Close();
                }
            }
            if (!dalInfo.LongConnection)
            {
                if (conntion.State != ConnectionState.Closed)
                    conntion.Close();
                conntion.Dispose();
            }
        }
    }
}
