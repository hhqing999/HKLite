using System;
using System.Data;
using System.Data.Common;

using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HKLite
{
    internal class SqlHelper
    {
        public static int SqlCommandTimeOut = 0;

        public static int ExecuteNonQuery(DbConnection conntion, bool longConnection, string cmdText, params DbParameter[] cmdParams)
        {
            return ExecuteNonQuery(conntion, longConnection, CommandType.Text, cmdText, cmdParams);
        }

        public static int ExecuteNonQuery(DbConnection conntion, bool longConnection, CommandType commandType, string cmdText, params DbParameter[] cmdParams)
        {
            return ExecuteNonQuery(null,false, conntion, longConnection, commandType, cmdText, cmdParams);
        }

        public static int ExecuteNonQuery(DbTransaction trans,bool autoRollback, DbConnection conntion, bool longConnection, CommandType commandType, string cmdText, params DbParameter[] cmdParams)
        {
            DbCommand cmd = conntion.CreateCommand();
            try
            {
                //通过PrePareCommand方法将参数逐个加入到DbCommand的参数集合中
                PrepareCommand(cmd, conntion, trans, commandType, cmdText, cmdParams);
                if (conntion.State != ConnectionState.Open)
                    conntion.Open();
                int val = cmd.ExecuteNonQuery();

                //清空DbCommand中的参数列表               
                return val;
            }
            catch (Exception ex)
            {
                // 当使用事务出错且自动回滚时，销毁事务，非长连接则销毁当前连接
                if (trans != null && autoRollback)
                {                   
                    trans.Rollback();
                    trans.Dispose();
                    if (!longConnection)
                    {
                        if (conntion.State != ConnectionState.Closed)
                            conntion.Close();
                        conntion.Dispose();
                    }
                }
                throw ex;
            }
            finally
            {
                if (!longConnection)
                {
                    // 非长连接且不使用事务时，执行完成后销毁当前连接
                    if (trans == null)
                    {
                        if (conntion.State != ConnectionState.Closed)
                            conntion.Close();
                        conntion.Dispose();
                    }
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, string[] cmdText)
        {
            return ExecuteTransac(conntion, longConnection, cmdText, null);
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, CommandType[] commandType, string[] cmdText)
        {
            return ExecuteTransac(conntion, longConnection, commandType, cmdText, null);
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, List<string> cmdText)
        {
            return ExecuteTransac(conntion, longConnection, cmdText, null);
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, List<CommandType> commandType, List<string> cmdText)
        {
            return ExecuteTransac(conntion, longConnection, commandType, cmdText, null);
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, string[] cmdText, DbParameter[][] cmdParams)
        {
            CommandType[] commandType = new CommandType[cmdText.Length];
            for (int i = 0; i < commandType.Length; i++)
            {
                commandType[i] = CommandType.Text;
            }
            return ExecuteTransac(conntion, longConnection, commandType, cmdText, cmdParams);
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, CommandType[] commandType, string[] cmdText, DbParameter[][] cmdParams)
        {
            DbCommand cmd = conntion.CreateCommand();
            DbTransaction transaction = null;

            try
            {
                if (conntion.State != ConnectionState.Open)
                    conntion.Open();
                transaction = conntion.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.Connection = conntion;
                if (SqlCommandTimeOut > 0)
                    cmd.CommandTimeout = SqlCommandTimeOut;

                for (int i = 0; i < cmdText.Length; i++)
                {
                    if (cmdText[i] == null || cmdText[i].Trim().Length == 0) continue;
                    cmd.CommandType = commandType[i];
                    cmd.CommandText = cmdText[i];

                    if (cmdParams != null)
                    {
                        DbParameter[] p = cmdParams[i];
                        if (p != null && p.Length > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(p);
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (!longConnection)
                {
                    if (conntion.State != ConnectionState.Closed)
                        conntion.Close();
                    conntion.Dispose();
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, List<string> cmdText, List<DbParameter[]> cmdParams)
        {
            List<CommandType> commandType = new List<CommandType>();
            for (int i = 0; i < cmdText.Count; i++)
            {
                commandType.Add(CommandType.Text);
            }
            return ExecuteTransac(conntion, longConnection, commandType, cmdText, cmdParams);
        }

        public static bool ExecuteTransac(DbConnection conntion, bool longConnection, List<CommandType> commandType, List<string> cmdText, List<DbParameter[]> cmdParams)
        {
            DbCommand cmd = conntion.CreateCommand();
            DbTransaction transaction = null;

            try
            {
                if (conntion.State != ConnectionState.Open)
                    conntion.Open();
                transaction = conntion.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.Connection = conntion;
                if (SqlCommandTimeOut > 0)
                    cmd.CommandTimeout = SqlCommandTimeOut;

                for (int i = 0; i < cmdText.Count; i++)
                {
                    if (cmdText[i] == null || cmdText[i].Trim().Length == 0) continue;
                    cmd.CommandType = commandType[i];
                    cmd.CommandText = cmdText[i];

                    if (cmdParams != null)
                    {
                        DbParameter[] p = cmdParams[i];
                        if (p != null && p.Length > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(p);
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (!longConnection)
                {
                    if (conntion.State != ConnectionState.Closed)
                        conntion.Close();
                    conntion.Dispose();
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        public static void ExecuteReader(ReaderDelegateHandler DBReader, DbConnection conntion, bool longConnection, string cmdText, params DbParameter[] cmdParams)
        {
            ExecuteReader(DBReader, conntion, longConnection, CommandType.Text, cmdText, cmdParams);
        }

        public static void ExecuteReader(ReaderDelegateHandler DBReader, DbConnection conntion, bool longConnection, CommandType commandType, string cmdText, params DbParameter[] cmdParams)
        {
            DbCommand cmd = conntion.CreateCommand();
            try
            {
                PrepareCommand(cmd, conntion, null, commandType, cmdText, cmdParams);
                if (conntion.State != ConnectionState.Open)
                    conntion.Open();
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    if (DBReader != null) DBReader(dr);
                    if (!dr.IsClosed)
                        dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!longConnection)
                {
                    if (conntion.State != ConnectionState.Closed)
                        conntion.Close();
                    conntion.Dispose();
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        public static DataTable ExecuteDataTable(DbConnection conntion, IDbDataAdapter da, bool longConnection, string cmdText, params DbParameter[] cmdParams)
        {
            return ExecuteDataTable(conntion, da, longConnection, CommandType.Text, cmdText, cmdParams);
        }

        public static DataTable ExecuteDataTable(DbConnection conntion, IDbDataAdapter da, bool longConnection, CommandType commandType, string cmdText, params DbParameter[] cmdParams)
        {
            DataSet ds = new DataSet();
            DbCommand cmd = conntion.CreateCommand();
            try
            {
                PrepareCommand(cmd, conntion, null, commandType, cmdText, cmdParams);                
                DataTable dt = new DataTable();
                if (conntion.State != ConnectionState.Open)
                    conntion.Open();
                //DbDataReader dr = cmd.ExecuteReader();
                //dt.Load(dr);
                //if (!dr.IsClosed)
                //    dr.Close();

               // IDbDataAdapter da =;
                da.SelectCommand = cmd;
                da.Fill(ds);
                dt = ds.Tables[0].Copy();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!longConnection)
                {
                    if (conntion.State != ConnectionState.Closed)
                        conntion.Close();
                    conntion.Dispose();
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                ds.Clear();
                ds.Dispose();
            }
        }

        public static object ExecuteScalar(DbConnection conntion, bool longConnection, string cmdText, params DbParameter[] cmdParams)
        {
            return ExecuteScalar(conntion, longConnection, CommandType.Text, cmdText, cmdParams);
        }

        public static object ExecuteScalar(DbConnection conntion, bool longConnection, CommandType commandType, string cmdText, params DbParameter[] cmdParams)
        {

            DbCommand cmd = conntion.CreateCommand();

            try
            {
                PrepareCommand(cmd, conntion, null, commandType, cmdText, cmdParams);
                if (conntion.State != ConnectionState.Open)
                    conntion.Open();
                object val = cmd.ExecuteScalar();

                return val;
            }
            finally
            {
                if (!longConnection)
                {
                    if (conntion.State != ConnectionState.Closed)
                        conntion.Close();
                    conntion.Dispose();
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }


        private static void PrepareCommand(DbCommand cmd, DbConnection conntion, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParams)
        {
            cmd.Connection = conntion;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;
            if (SqlCommandTimeOut > 0)
                cmd.CommandTimeout = SqlCommandTimeOut;

            if (cmdParams != null)
            {
                foreach (DbParameter parm in cmdParams)
                {
                    if (null != parm)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }

            }
        }



    }
}
