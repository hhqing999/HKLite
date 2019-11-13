using System;

using System.Collections.Generic;
using System.Text;

using System.Data ;
using System .Data.Common ;

namespace HKLite
{
    internal class DalLayerInfo
    {
        private string connectionString;
        private DbConnection connection;
        private bool longConnection;
        private IDbProvider provider;

        private object lockObj = new object();

        public DalLayerInfo(string connectionString, bool longConnection, IDbProvider provider)
        {
            this.connectionString = connectionString;
            this.longConnection = longConnection;
            this.provider = provider;
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public DbConnection Connection
        {
            get
            {
                if (longConnection)
                {
                    if (connection == null)
                    {
                        lock (lockObj)
                        {
                            if (connection == null)
                                connection = provider.CreateConnection(connectionString);
                        }
                    }
                    return connection;
                }
                else
                {
                    return provider.CreateConnection(connectionString);
                }
            }
        }

        public bool LongConnection
        {
            get { return longConnection; }
        }

        public IDbProvider Provider
        {
            get { return provider; }
        }
    }
}
