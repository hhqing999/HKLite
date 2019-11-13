using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;

using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace HKLite
{
    internal class TransacBuilder : ITransacBuilder
    {
        internal delegate void RunDelegate();
        internal RunDelegate BeforeRun;

        private DalLayerInfo dalInfo;

        private List<CommandType> commandTypes;  
        private List<string> cmdTexts;      
        private List<DbParameter[]> cmdParams;
        // 主键集合<表名,主键列名>
        private Dictionary<string, string> dicKeyNames = new Dictionary<string, string>();

        // 自增字段集合<表名,自增字段名>
        private Dictionary<string, string> dicIdentityNames = new Dictionary<string, string>();
        private Dictionary<string, object> dicBuilder = new Dictionary<string, object>();

        /// <summary>
        /// 构造方法
        /// </summary>
        public TransacBuilder(DalLayerInfo dalInfo, Dictionary<string, string> dicKeyNames, Dictionary<string, string> dicIdentityNames)
        {
            this.dalInfo = dalInfo;
            this.dicKeyNames = dicKeyNames;
            this.dicIdentityNames = dicIdentityNames;
            this.commandTypes = new List<CommandType>();
            this.cmdTexts = new List<string>();
            this.cmdParams = new List<DbParameter[]>();
        }

        #region public
        public IDao<T> Dao<T>()
        {
            return new Dao<T>(this);
        }

        public IQueryBuilder<T> QueryBuilder<T>()
        {
            string entityName = typeof(T).Name;          
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
            string key = string.Format("IInsertBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new InsertBuilder<T>(dalInfo, GetKeyName(entityName), GetIdentityName(entityName), this);
                dicBuilder.Add(key, value);
            }
            return (InsertBuilder<T>)value;
        }

        public IUpdateBuilder<T> UpdateBuilder<T>()
        {
            string entityName = typeof(T).Name;
            string key = string.Format("IUpdateBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new UpdateBuilder<T>(dalInfo, GetKeyName(entityName), this);
                dicBuilder.Add(key, value);
            }
            return (UpdateBuilder<T>)value;
        }

        public IDeleteBuilder<T> DeleteBuilder<T>()
        {
            string entityName = typeof(T).Name;
            string key = string.Format("IDeleteBuilder_{0}", entityName);
            object value = null;
            if (!dicBuilder.TryGetValue(key, out value))
            {
                value = new DeleteBuilder<T>(dalInfo, GetKeyName(entityName), this);
                dicBuilder.Add(key, value);
            }
            return (DeleteBuilder<T>)value;
        }

        public void AddCommandText(string cmdText)
        {
            if (cmdText != null && cmdText.Trim().Length > 0)
            {
                AddCommondType(CommandType.Text);
                AddCmdText(cmdText);
                AddCmdParam(null);
            }
        }

        public void AddCommandText(CommandType commandType, string cmdText, params DbParameter[] param)
        {
            if (cmdText != null && cmdText.Trim().Length > 0)
            {
                AddCommondType(commandType);
                AddCmdText(cmdText);
                AddCmdParam(param);
            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
            try
            {
                if (this.BeforeRun != null)
                    this.BeforeRun();

                if (cmdTexts.Count == 0)
                    return true;

                return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, commandTypes, cmdTexts, cmdParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.cmdTexts.Clear();
                this.cmdParams.Clear();
            }
        }

               
        #endregion

        #region internal
        internal int GetCmdTextsCount()
        {
            return cmdTexts.Count;
        }

        internal void AddCommondType(CommandType commandType)
        {
            commandTypes.Add(commandType);
        }

        internal void AddCmdText(string cmdText)
        {
            cmdTexts.Add(cmdText);
        }

        internal void InsertCmdText(int index, string cmdText)
        {
            cmdTexts.Insert(index, cmdText);
        }

        internal void UpdateCmdText(int index, string cmdText)
        {
            cmdTexts[index] = cmdText;
        }

        internal void AddCmdParam(DbParameter[] rparam)
        {
            cmdParams.Add(rparam);
        }

        internal void InsertCmdParam(int index, DbParameter[] rparam)
        {
            cmdParams.Insert(index, rparam);
        }

        internal void UpdateCmdParam(int index, DbParameter[] rparam)
        {
            cmdParams[index] = rparam;
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








    }
}
