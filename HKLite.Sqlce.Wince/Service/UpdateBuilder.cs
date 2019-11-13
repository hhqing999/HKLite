using System;

using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace HKLite
{
    internal class UpdateBuilder<T> : IUpdateBuilder<T> 
    {
        private DalLayerInfo dalInfo;
        private TransacBuilder tranBuilder;
        private string keyName;

        private List<string> cmdTexts = new List<string>();
        private List<DbParameter[]> cmdParams = new List<DbParameter[]>();     
        private List<UpdateObject> listUpdateObject = new List<UpdateObject>();
       
        private Update_ myUpdate = new Update_();

        internal UpdateBuilder(DalLayerInfo dalInfo, string keyName)
        {
            SetBuilderParams(dalInfo, keyName, null);
        }

        internal UpdateBuilder(DalLayerInfo dalInfo, string keyName, TransacBuilder tranBuilder)
        {
            SetBuilderParams(dalInfo, keyName, tranBuilder);
        }

        private void SetBuilderParams(DalLayerInfo dalInfo, string keyName, TransacBuilder tranBuilder)
        {
            this.dalInfo = dalInfo;
            this.keyName = keyName;
            if (tranBuilder != null)
            {
                this.tranBuilder = tranBuilder;
                this.tranBuilder.BeforeRun += this.CreateSqlCommand;
            }
        }

        #region public
        public Update_ Update()
        {
            InitSet();
            AddUpdateObject(null, default(T), this.myUpdate.MySet);
            return this.myUpdate;
        }

        /// <summary>
        /// 根据主键值更新，entity中主键字段和需要更新的字段不为null，不更新字段全为null
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateByKey(T entity)
        {
            UpdateByKey(entity, null);
        }

        public void UpdateByKey( T entity, params string[] excludeColumns)
        {
            if (this.keyName == null || this.keyName.Trim().Length == 0)
                throw new Exception(string.Format("表{0}不存在主键", keyName)); 
            if (entity == null)
                throw new Exception("更新的实体不能为null");  

            InitSet();
            AddUpdateObject(keyName, entity, null,excludeColumns);
        }


        /// <summary>
        /// 执行更新操作(只执行首条SQL语句)
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
                foreach (UpdateObject item in listUpdateObject)
                {
                    string cmd = GetEntityCmd(item);
                    DbParameter[] rparam = GetEntityParams(item);
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

            if (this.listUpdateObject.Count > 0)
            {
                foreach (UpdateObject item in listUpdateObject)
                {
                    string cmd = GetEntityCmd(item);
                    DbParameter[] rparam = GetEntityParams(item);
                    if (tranBuilder != null)
                    {
                        // 填充内容
                        tranBuilder.UpdateCmdText(item.CmdIndex, cmd);
                        tranBuilder.UpdateCmdParam(item.CmdIndex, rparam);
                    }
                    else
                    {
                        this.cmdTexts.Add(cmd);
                        this.cmdParams.Add(rparam);
                    }
                }
                listUpdateObject.Clear();
            }           
        }

        private void InitSet()
        {
            myUpdate = new Update_();           
        }

        private void AddUpdateObject(string keyName, T entity, Set_ updateSet)
        {
            AddUpdateObject(keyName, entity, updateSet, null);
        }

        private void AddUpdateObject(string keyName,T entity,Set_ updateSet,params string[] excludeColumns)
        {
            UpdateObject updateObject=new UpdateBuilder<T>.UpdateObject ();
         
            if (keyName != null && keyName.Trim().Length > 0)
            {
                Dictionary<string, object> dit = new Dictionary<string, object>();
                Where_ tempWhere = new Where_();

                object[] objs = null;
                PropertyInfo []pis=typeof (T).GetProperties ();
                bool isKeyValueNull=true ; // 主键值是否为null
                for (int i = 0; i < pis.Length; i++)
                {
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

                    if (pis[i].Name.ToLower() == keyName.Trim().ToLower())
                    {
                        isKeyValueNull = false;
                        tempWhere.Equal(keyName, value);
                        continue;
                    }

                    dit.Add(pis[i].Name, value);
                }

                if (isKeyValueNull)
                {
                    throw new Exception(string.Format("属性{0}的值不能为null", keyName));
                }

                updateObject.Where = tempWhere;
                updateObject.DitUpdateInfo = dit;
            }
            else if (updateSet != null)
            {
                updateObject.DitUpdateInfo = updateSet.DitUpdateInfo;
                updateObject.Where = updateSet.MyWhere;
            }

            if (tranBuilder != null)
            {
                updateObject.CmdIndex = tranBuilder.GetCmdTextsCount();

                // 在集合中站位，等创建脚本的时候再将内容填充
                tranBuilder.AddCommondType(CommandType.Text);
                tranBuilder.AddCmdText(null);
                tranBuilder.AddCmdParam(null);
            }
            else
            {
                // 不是事务操作时，一次只操作一条语句
                this.listUpdateObject.Clear();
                updateObject.CmdIndex = listUpdateObject.Count;
            }
            this.listUpdateObject.Add(updateObject);
        }

        /// <summary>
        /// 被更新字段参数名加上前缀"u", 如 Name=@uName，以此来使其与条件参数不同
        /// </summary>
        /// <param name="updateObject"></param>
        /// <returns></returns>
        private string GetEntityCmd(UpdateObject updateObject)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("UPDATE ");
            sbText.Append(typeof(T).Name);
            sbText.Append(" SET ");
          
            if (updateObject != null)
            {
                int n = 0;
                foreach (KeyValuePair<string, object> item in updateObject.DitUpdateInfo)
                {
                    if (n != 0)
                        sbText.Append(",");
                    sbText.Append(item.Key);
                    sbText.Append("=@u");
                    sbText.Append(item.Key);

                    n++;
                }

                if (updateObject.Where != null)
                {
                    sbText.Append(" ");
                    sbText.Append(Common.GetWhereText(updateObject.Where));
                }
            }

            sbText.Append(";");
            return sbText.ToString();
        }

        private DbParameter[] GetEntityParams(UpdateObject updateObject)
        {
            List<DbParameter >list=new List<DbParameter> ();

            // 更新字段参数
            if (updateObject.DitUpdateInfo.Count > 0)
            {
                foreach (KeyValuePair<string, object> item in updateObject.DitUpdateInfo)
                {
                    list.Add(dalInfo.Provider.CreateParameter(string.Format("@u{0}", item.Key), item.Value));
                }
            }

            // 条件参数                
            if (updateObject.Where.ListWhereEntity != null && updateObject.Where.ListWhereEntity.Count > 0)
            {
                foreach (WhereEntity item in updateObject.Where.ListWhereEntity)
                {
                    if (item.RelationType == RelationTypeEnum.IsNull)
                        continue;
                    list.Add(dalInfo.Provider.CreateParameter(string.Format("@{0}", item.ColumnName), item.Value));
                }
            }

            if (list.Count == 0)
                return null;
            return list.ToArray();
        }
        #endregion

        private class UpdateObject
        {
           public int CmdIndex;

           public Dictionary<string, object> DitUpdateInfo;

           public Where_ Where;

        }


    }
}
