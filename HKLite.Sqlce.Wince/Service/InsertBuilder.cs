using System;

using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace HKLite
{
    internal class InsertBuilder<T> : IInsertBuilder<T>
    {
        private DalLayerInfo dalInfo;
        private TransacBuilder tranBuilder;
        private string keyName;

        private List<string> cmdTexts = new List<string>();
        private List<DbParameter[]> cmdParams = new List<DbParameter[]>();
        private List<InsertObject> listInsertObject = new List<InsertObject>();
        private string identityName;

        internal InsertBuilder(DalLayerInfo dalInfo, string keyName, string identityName)
        {
            SetBuilderParams(dalInfo, keyName, identityName, null);
        }

        internal InsertBuilder(DalLayerInfo dalInfo, string keyName, string identityName, TransacBuilder tranBuilder)
        {
            SetBuilderParams(dalInfo, keyName, identityName, tranBuilder);
        }

        private void SetBuilderParams(DalLayerInfo dalInfo, string keyName, string identityName, TransacBuilder tranBuilder)
        {
            this.dalInfo = dalInfo;
            this.keyName = keyName;
            this.identityName = identityName;
            if (tranBuilder != null)
            {
                this.tranBuilder = tranBuilder;
                this.tranBuilder.BeforeRun += this.CreateSqlCommand;
            }
        }

        #region public
        public void Insert(T entity, params string[] excludeColumns)
        {
            if (entity == null)
                return;

            AddCmds(entity, excludeColumns);
        }

        public void Insert(T entity)
        {
            Insert(entity, null);
        }


        /// <summary>
        /// 执行插入操作(只执行首条SQL语句)
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        public int Run()
        {
            int result = 0;
            if (tranBuilder != null)
                throw new Exception("执行事务应调用ITransacBuilder中相应的方法");
            CreateSqlCommand();

            try
            {
                if (this.cmdTexts.Count > 0)
                {
                    result = SqlHelper.ExecuteNonQuery(dalInfo.Connection, dalInfo.LongConnection, cmdTexts[0], cmdParams[0]);
                }
                return result;
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

        public List<string> SqlCommands
        {
            get
            {
                List<string> list = new List<string>();
                foreach (InsertObject item in listInsertObject)
                {
                    string cmd = item.CmdText;
                    DbParameter[] rparam = item.CmdParamrters;
                    if (rparam != null)
                    {
                        int n = 0;
                        foreach (var p in rparam)
                        {
                            cmd = string.Format("{0}{1}{2}:{3}", cmd, n > 0 ? "," : "", p.ParameterName, p.Value);
                            n++;
                        }
                    }
                    list.Add(cmd);
                }
                return list;
            }
        }

        #endregion


        #region private
        /// <summary>
        /// 创建脚本
        /// </summary>
        private void CreateSqlCommand()
        {
            this.cmdTexts.Clear();
            this.cmdParams.Clear();

            if (this.listInsertObject.Count > 0)
            {
                foreach (InsertObject item in listInsertObject)
                {
                    if (tranBuilder != null)
                    {
                        // 填充内容
                        tranBuilder.UpdateCmdText(item.CmdIndex, item.CmdText);
                        tranBuilder.UpdateCmdParam(item.CmdIndex, item.CmdParamrters);
                    }
                    else
                    {
                        this.cmdTexts.Add(item.CmdText);
                        this.cmdParams.Add(item.CmdParamrters);
                    }
                }
                listInsertObject.Clear();
            }
        }

        private string GetEntityCmd(int cmdIndex, string[] columns)
        {
            StringBuilder sbText = new StringBuilder();

            sbText.Append("INSERT INTO ");
            sbText.Append(typeof(T).Name);
            sbText.Append("(");
            sbText.Append(GetParamColumns(false, cmdIndex, columns));
            sbText.Append(") VALUES (");
            sbText.Append(GetParamColumns(true, cmdIndex, columns));
            sbText.Append(");");

            return sbText.ToString();
        }

        private DbParameter[] GetEntityParams(T entity, int cmdIndex, string[] columns, object[] values)
        {
            DbParameter[] rparam = null;

            rparam = new DbParameter[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                rparam[i] = dalInfo.Provider.CreateParameter(string.Format("@{0}", columns[i]), values[i]);
            }
            return rparam;
        }

        private void AddCmds(T entity, params string[] excludeColumns)
        {
            string[] columns = null;
            object[] values = null;
            object[] objs = null;
            List<string> listColumn = new List<string>();
            List<object> listValue = new List<object>();
            PropertyInfo[] pis = typeof(T).GetProperties();

            for (int i = 0; i < pis.Length; i++)
            {
                // 排除插入列
                if (excludeColumns != null && excludeColumns.Length > 0)
                {
                    bool isExclude = false;
                    foreach (string excludeColumn in excludeColumns)
                    {
                        if (pis[i].Name.ToLower() == excludeColumn.Trim().ToLower())
                        {
                            isExclude = true;
                            break;
                        }
                    }
                    if (isExclude)
                        continue;
                }

                // 排除自增字段
                if (identityName != null)
                    if (pis[i].Name.ToLower() == identityName.ToLower())
                        continue;

                // 排除只显示列
                objs = pis[i].GetCustomAttributes(false);
                if (objs != null && objs.Length > 0)
                {
                    EntityAttribute cusatt = (EntityAttribute)objs[0];
                    if (cusatt.DisplayOnly)
                    {
                        continue;
                    }
                }

                object value = pis[i].GetValue(entity, null);
                if (value == null)
                    continue;
                listColumn.Add(pis[i].Name);
                listValue.Add(value);
            }
            if (listColumn.Count > 0)
            {
                columns = listColumn.ToArray();
                values = listValue.ToArray();
            }
            if (columns == null)
                return;

            int cmdIndex;
            if (tranBuilder == null)
            {
                // 不是事务操作时，一次只操作一条语句
                this.listInsertObject.Clear();
                cmdIndex = this.cmdTexts.Count;
            }
            else
            {
                cmdIndex = tranBuilder.GetCmdTextsCount();

                // 在集合中站位，等创建脚本的时候再将内容填充
                tranBuilder.AddCommondType(CommandType.Text);
                tranBuilder.AddCmdText(null);
                tranBuilder.AddCmdParam(null);
            }

            listInsertObject.Add(
                new InsertBuilder<T>.InsertObject
                {
                    CmdIndex = cmdIndex,
                    CmdText = GetEntityCmd(cmdIndex, columns),
                    CmdParamrters = GetEntityParams(entity, cmdIndex, columns, values),
                });
        }



        private string GetParamColumns(bool isAddTag, int cmdIndex, string[] columns)
        {
            string columnsStr = "";

            for (int i = 0; i < columns.Length; i++)
            {
                if (isAddTag)
                    columnsStr = string.Format("{0}@{1},", columnsStr, columns[i]);
                else
                    columnsStr = string.Format("{0}{1},", columnsStr, columns[i]);
            }
            return columnsStr.Trim(',');
        }

        private class InsertObject
        {
            public int CmdIndex;

            public string CmdText;

            public DbParameter[] CmdParamrters;
        }
        #endregion




    }
}
