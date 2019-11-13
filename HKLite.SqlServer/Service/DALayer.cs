using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Data.Common;
namespace HKLite
{
    internal class DALayer : IDALayer
    {
        // 初始化时线程ID
        private int initThreadId;
        private bool longConnection;
        private IDbProvider provider;
        private Type[] entityTypes;
       
        private string connectionString;

        private DalLayerInfo dalInfo;

        // 根据数据库名称取得 规则：db_dbname
        private string dbMark;

        // 主键集合<表名,主键列名>
        private Dictionary<string, string> dicKeyNames = new Dictionary<string, string>();

        // 自增字段集合<表名,自增字段名>
        private Dictionary<string, string> dicIdentityNames = new Dictionary<string, string>();
        private Dictionary<string, object> dicBuilder = new Dictionary<string, object>();

        internal DALayer(string connectString, string entityDllName, bool longConnection, IDbProvider provider, int? commandTimeOut)
        {
            this.connectionString = connectString;

            Assembly assemTemp = Assembly.LoadFrom(entityDllName);
            this.entityTypes = assemTemp.GetTypes();

            this.dbMark = string.Format("db_{0}", entityDllName.Replace(".", "_").ToLower());
            this.longConnection = longConnection;
            this.provider = provider;
            if (commandTimeOut != null && commandTimeOut > 0)
                SqlHelper.SqlCommandTimeOut = (int)commandTimeOut;
        }
 
        #region public

        public bool CreateTables()
        {
            DBHelper dbHelper = new DBHelper();
            dbHelper.CreateTables(dalInfo, this.entityTypes);
            return true;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        public bool Init(ref string msg)
        {
            try
            {
                this.initThreadId = Thread.CurrentThread.ManagedThreadId;

                this.dalInfo = new DalLayerInfo(this.connectionString, this.longConnection, this.provider);
                
                // 测试数据库连接
                if (string.IsNullOrEmpty(connectionString.Trim()))
                {
                    msg = "连接字符串为空";
                    return false;
                }
                object obj = SqlHelper.ExecuteScalar(provider.CreateConnection(connectionString), false, "select 1");
                if (obj == null || obj == DBNull.Value)
                    return false;
                if (Convert.ToInt32(obj) != 1)
                    return false;

                // 获取表主键及自增字段
                object[] objs;
                object[] customAttributes;
                Type attType = typeof(EntityAttribute);
                foreach (Type entityType in this.entityTypes)
                {
                    bool isEntity = false;
                    customAttributes = entityType.GetCustomAttributes(false);
                    if (customAttributes != null)
                    {
                        foreach (var a in customAttributes)
                        {
                            if (a.GetType() == attType)
                            {
                                isEntity = true;
                                break;
                            }
                        }
                    }
                    if (!isEntity)
                        continue;

                    // 找到目标次数
                    int findCount = 0;
                    foreach (PropertyInfo pinfo in entityType.GetProperties())
                    {
                        findCount = 0;
                        objs = pinfo.GetCustomAttributes(false);
                        if (objs != null && objs.Length > 0)
                        {
                            EntityAttribute cusatt = (EntityAttribute)objs[0];
                            if (cusatt.PrimaryKey)
                            {
                                dicKeyNames.Add(entityType.Name, pinfo.Name);
                                findCount++;
                            }
                            if (cusatt.Identity)
                            {
                                dicIdentityNames.Add(entityType.Name, pinfo.Name);
                                findCount++;
                            }
                            if (findCount == 2)
                                break;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }


        public void Open()
        {
            if (dalInfo.LongConnection)
                if (dalInfo.Connection.State != ConnectionState.Open)
                    dalInfo.Connection.Open();
        }

        public void Close()
        {
            if (dalInfo.LongConnection)
                if (dalInfo.Connection.State != ConnectionState.Closed)
                    dalInfo.Connection.Close();
        }

        public void SyncTables(ITransacBuilder transacBuilder)
        {
            DBHelper dbHelper = new DBHelper();
            dbHelper.SyncTables(transacBuilder, dalInfo, entityTypes);
        }

        public IDao<T> Dao<T>()
        {
            return new Dao<T>(this);
        }

        public IQueryBuilder<T> QueryBuilder<T>()
        {
            string entityName = typeof(T).Name;
            if (Thread.CurrentThread.ManagedThreadId != this.initThreadId)
                return new QueryBuilder<T>(dalInfo, GetKeyName(entityName));
            string key = string.Format("IQueryBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new QueryBuilder<T>(dalInfo, GetKeyName(entityName));
                dicBuilder.Add(key, value);
            }
            return (QueryBuilder<T>)value;
        }

        public IInsertBuilder<T> InsertBuilder<T>()
        {
            string entityName = typeof(T).Name;
            if (Thread.CurrentThread.ManagedThreadId != this.initThreadId)
                return new InsertBuilder<T>(dalInfo, GetKeyName(entityName), GetIdentityName(entityName));
            string key = string.Format("IInsertBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new InsertBuilder<T>(dalInfo, GetKeyName(entityName), GetIdentityName(entityName));
                dicBuilder.Add(key, value);
            }
            return (InsertBuilder<T>)value;
        }

        public IUpdateBuilder<T> UpdateBuilder<T>()
        {
            string entityName = typeof(T).Name;
            if (Thread.CurrentThread.ManagedThreadId != this.initThreadId)
                return new UpdateBuilder<T>(dalInfo, GetKeyName(entityName));
            string key = string.Format("IUpdateBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new UpdateBuilder<T>(dalInfo, GetKeyName(entityName));
                dicBuilder.Add(key, value);
            }
            return (UpdateBuilder<T>)value;
        }

        public IDeleteBuilder<T> DeleteBuilder<T>()
        {
            string entityName = typeof(T).Name;
            if (Thread.CurrentThread.ManagedThreadId != this.initThreadId)
                return new DeleteBuilder<T>(dalInfo, GetKeyName(entityName));
            string key = string.Format("IDeleteBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new DeleteBuilder<T>(dalInfo, GetKeyName(entityName));
                dicBuilder.Add(key, value);
            }
            return (DeleteBuilder<T>)value;
        }

        public ITransacBuilder TransacBuilder()
        {
            if (Thread.CurrentThread.ManagedThreadId != this.initThreadId)
                return new TransacBuilder(dalInfo, dicKeyNames, dicIdentityNames);
            string key = "ITransacBuilder";
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new TransacBuilder(dalInfo, dicKeyNames, dicIdentityNames);
                dicBuilder.Add(key, value);
            }
            return (TransacBuilder)value;
        }

        public HKLiteTransaction GetTransaction()
        {
            DbConnection conn = dalInfo.Connection;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            var dbTrans = conn.BeginTransaction();
            return new HKLiteTransaction(dbTrans, dalInfo.LongConnection);
        }

        public int ExecuteNonQuery(string cmdText)
        {
            return SqlHelper.ExecuteNonQuery(dalInfo.Connection, dalInfo.LongConnection, cmdText, null);
        }

        public int ExecuteNonQuery(CommandType commandType, string cmdText)
        {
            return SqlHelper.ExecuteNonQuery(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdText, null);
        }

        public int ExecuteNonQuery(CommandType commandType, string cmdText, params DbParameter[] param)
        {
            return SqlHelper.ExecuteNonQuery(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdText, param);
        }

        public int ExecuteNonQuery(HKLiteTransaction trans, string cmdText)
        {
            return ExecuteNonQuery(trans, CommandType.Text, cmdText, null);
        }

        public int ExecuteNonQuery(HKLiteTransaction trans, string cmdText, params DbParameter[] param)
        {
            return ExecuteNonQuery(trans, CommandType.Text, cmdText, param);
        }

        public int ExecuteNonQuery(HKLiteTransaction trans, CommandType commandType, string cmdText, params DbParameter[] param)
        {
            DbConnection conn = null;
            if (trans != null)
                conn = trans.DbTransaction.Connection;
            else
                conn = dalInfo.Connection;
            return SqlHelper.ExecuteNonQuery(trans.DbTransaction, false, conn, dalInfo.LongConnection, commandType, cmdText, param);
        }

        public object ExecuteScalar(string cmdText)
        {
            return SqlHelper.ExecuteScalar(dalInfo.Connection, dalInfo.LongConnection, cmdText, null);
        }

        public object ExecuteScalar(CommandType commandType, string cmdText)
        {
            return SqlHelper.ExecuteScalar(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdText, null);
        }

        public object ExecuteScalar(CommandType commandType, string cmdText, params DbParameter[] param)
        {
            return SqlHelper.ExecuteScalar(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdText, param);
        }

        public bool ExecuteTransac(string[] cmdText)
        {
            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, cmdText);
        }

        public bool ExecuteTransac(CommandType[] commandType, string[] cmdText)
        {
            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdText);
        }

        public bool ExecuteTransac(CommandType[] commandType, string[] cmdText, DbParameter[][] param)
        {
            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdText, param);
        }

        public bool ExecuteTransac(List<string> cmdTexts)
        {
            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, cmdTexts);
        }

        public bool ExecuteTransac(List<CommandType> commandType, List<string> cmdTexts)
        {
            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdTexts);
        }

        public bool ExecuteTransac(List<CommandType> commandType, List<string> cmdTexts, List<DbParameter[]> param)
        {
            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, commandType, cmdTexts, param);
        }

        public DataTable ExecuteTable(string cmdText)
        {
            return SqlHelper.ExecuteDataTable(dalInfo.Connection, dalInfo.Provider.CreateDataAdapter(), dalInfo.LongConnection, cmdText, null);
        }

        public DataTable ExecuteTable(CommandType commandType, string cmdText)
        {
            return SqlHelper.ExecuteDataTable(dalInfo.Connection, dalInfo.Provider.CreateDataAdapter(), dalInfo.LongConnection, commandType, cmdText, null);
        }

        public DataTable ExecuteTable(CommandType commandType, string cmdText, params DbParameter[] param)
        {
            return SqlHelper.ExecuteDataTable(dalInfo.Connection, dalInfo.Provider.CreateDataAdapter(), dalInfo.LongConnection, commandType, cmdText, param);
        }

        public void ExecuteReader(string sqlCommand, ReaderDelegateHandler dbReader)
        {
            SqlHelper.ExecuteReader(dbReader, dalInfo.Connection, dalInfo.LongConnection, sqlCommand, null);
        }

        public void ExecuteReader(CommandType commandType, string sqlCommand, ReaderDelegateHandler dbReader)
        {
            SqlHelper.ExecuteReader(dbReader, dalInfo.Connection, dalInfo.LongConnection, commandType, sqlCommand, null);
        }

        public void ExecuteReader(CommandType commandType, string sqlCommand, ReaderDelegateHandler dbReader, params DbParameter[] param)
        {
            SqlHelper.ExecuteReader(dbReader, dalInfo.Connection, dalInfo.LongConnection, commandType, sqlCommand, param);
        }


        #endregion

        #region private
        private string GetKeyName(string entityName)
        {
            string keyName = null;
            dicKeyNames.TryGetValue(entityName, out keyName);
            return keyName;
        }

        private string GetIdentityName(string entityName)
        {
            string identityName = null;
            dicIdentityNames.TryGetValue(entityName, out identityName);
            return identityName;
        }

        #endregion

        #region override
        //internal override DbConnection GetConnection()
        //{
        //    return this.dalInfo.Connection;
        //}

        //internal override DalLayerInfo GetDalyerInfo()
        //{
        //    return dalInfo;
        //}
        #endregion




    }
}
