using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace HKLite
{
    internal class DeleteBuilder<T> : IDeleteBuilder<T>
    {
        private DalLayerInfo dalInfo;
        private TransacBuilder tranBuilder;
        private string keyName;
       
        private List<string> cmdTexts = new List<string>();
        private List<DbParameter[]> cmdParams = new List<DbParameter[]>();     
        private Delete_ myDelete = null;
        private List<DeleteObject> listDeleteObject = new List<DeleteObject>();


        internal DeleteBuilder(DalLayerInfo dalInfo, string keyName)
        {
            SetBuilderParams(dalInfo,keyName, null);
        }

        internal DeleteBuilder(DalLayerInfo dalInfo, string keyName, TransacBuilder tranBuilder)
        {
            SetBuilderParams(dalInfo,keyName, tranBuilder);
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
        public Delete_ Delete()
        {
            this.myDelete = new Delete_();
            AddDeleteObject(this.myDelete);
            return this.myDelete;
        }

        public void DeleteByKey(object keyValue)
        {
            if (this.keyName == null || this.keyName.Trim().Length == 0)
                throw new Exception(string.Format("表{0}不存在主键", keyName));
            if (keyValue == null)
                throw new Exception("主键值不能为空");

            this.myDelete = new Delete_();
            this.myDelete.Where().Equal(keyName, keyValue);
            AddDeleteObject(this.myDelete);
        }

        /// <summary>
        /// 执行删除操作(只执行首条SQL语句)
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
                foreach (DeleteObject item in listDeleteObject)
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

        private void AddDeleteObject(Delete_  iDelete)
        {
            DeleteObject deleteObject = new DeleteBuilder<T>.DeleteObject();
            deleteObject.Delete = iDelete;

            if (tranBuilder != null)
            {
                deleteObject.CmdIndex = tranBuilder.GetCmdTextsCount();

                // 在集合中站位，等创建脚本的时候再将内容填充
                tranBuilder.AddCommondType(CommandType.Text);
                tranBuilder.AddCmdText(null);
                tranBuilder.AddCmdParam(null);
            }
            else
            {
                // 不是事务操作时，一次只操作一条语句
                this.listDeleteObject.Clear();
                deleteObject.CmdIndex = this.listDeleteObject.Count;
            }
            this.listDeleteObject.Add(deleteObject);
        }

        private string GetEntityCmd(DeleteObject dobj)
        {
            return string.Format("DELETE FROM {0} {1};", typeof(T).Name, Common.GetWhereText(dobj.Delete.MyWhere));
        }

        private DbParameter[] GetEntityParams(DeleteObject dobj)
        {
            // 条件参数  
            List<DbParameter> list = new List<DbParameter>();
            if (dobj.Delete.MyWhere != null && dobj.Delete.MyWhere.MyWhereRelation != null)
            {
                if (dobj.Delete.MyWhere.ListWhereEntity.Count > 0)
                {
                    foreach (WhereEntity item in dobj.Delete.MyWhere.ListWhereEntity)
                    {
                        if (item.RelationType == RelationTypeEnum.IsNull)
                            continue;
                        list.Add(dalInfo.Provider.CreateParameter(string.Format("@{0}", item.ColumnName), item.Value));
                    }
                }
            }

            if (list.Count == 0)
                return null;
            return list.ToArray();
        }
       

        private void CreateSqlCommand()
        {
            this.cmdTexts.Clear();
            this.cmdParams.Clear();

            if (this.listDeleteObject.Count > 0)
            {
                foreach (DeleteObject item in listDeleteObject)
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
                this.listDeleteObject.Clear();
            }
        }
        #endregion

        private class DeleteObject
        {
            public int CmdIndex;

           // public Where Where;

            public Delete_ Delete;
        }

    }
}
