using System;

using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace HKLite
{
    internal class QueryBuilder<T> : IQueryBuilder<T>
    {
        private DalLayerInfo dalInfo;
        private string keyName;
       
        private int top;
        private string[] columns;       
        private Select_ mySelect;
        private Where_ myWhere
        {
            get { return mySelect.MyWhere; }  
        }

        // 自定义脚本
        private string customSqlCommand;

        internal QueryBuilder(DalLayerInfo dalInfo, string keyName)
        {
            this.dalInfo = dalInfo;
            this.keyName = keyName;
        }

        #region public

        public string CustomSqlCommand
        {
            get { return customSqlCommand; }
            set
            {
                customSqlCommand = value;
                mySelect = null;
            }
        }

        public Select_ Select()
        {
            return Select(0, null);
        }

        public Select_ Select(params string[] columns)
        {
            return Select(0, columns);
        }

        public Select_ Select(int top)
        {
            return Select(top, null);
        }

        public Select_ Select(int top,params string[] columns)
        {
            this.customSqlCommand = null;
            this.top = top;
            this.columns = columns;
            mySelect = new Select_();
            return mySelect;
        }

        #region 执行查询操作
        /// <summary>
        /// 取DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable QueryTable()
        {
            return SqlHelper.ExecuteDataTable(dalInfo.Connection, dalInfo.Provider.CreateDataAdapter(), dalInfo.LongConnection, this.GetCmdText(), this.GetParameters());
        }

        public T QueryByKey(object keyValue)
        {
            if (this.keyName == null || this.keyName.Trim().Length == 0)
                throw new Exception(string.Format("表{0}不存在主键", keyName));
            this.Select().Where().Equal(keyName, keyValue);
            return QuerySingle();
        }

        /// <summary>
        /// 取单个实体类
        /// </summary>
        /// <returns></returns>
        public T QuerySingle()
        {
            T model = default(T);
            SqlHelper.ExecuteReader(dr =>
            {
                // 获取列集合
                List<string> listColumn = new List<string>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    listColumn.Add(dr.GetName(i).ToLower());
                }

                PropertyInfo[] pi = typeof(T).GetProperties();
                while (dr.Read())
                {
                    model = GetModel(dr, pi, listColumn);
                    break;
                }
            },
                this.dalInfo.Connection, dalInfo.LongConnection, this.GetCmdText(), this.GetParameters()
                );
            return model;
        }

        /// <summary>
        /// 取实体类集合
        /// </summary>
        /// <returns></returns>
        public List<T> Query()
        {
            List<T> list = new List<T>();
            SqlHelper.ExecuteReader(dr =>
            {
                // 获取列集合
                List<string> listColumn = new List<string>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    listColumn.Add(dr.GetName(i).ToLower());
                }

                PropertyInfo[] pi = typeof(T).GetProperties();
                T model = default(T);
                while (dr.Read())
                {
                    model = GetModel(dr, pi, listColumn);
                    list.Add(model);
                }
            },
                this.dalInfo.Connection, dalInfo.LongConnection, this.GetCmdText(), this.GetParameters()
                );
            if (list.Count == 0)
                return null;
            return list;
        }

        public List<T> QueryAll()
        {
            this.Select();
            return Query();
        }

        /// <summary>
        /// 取记录数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            if (!string.IsNullOrEmpty(customSqlCommand))
                throw new Exception("该方法不支持自定义脚本");

            string cmdText = string.Format("SELECT COUNT(0) FROM {0} {1};", typeof(T).Name, Common.GetWhereText(mySelect.MyWhere));
            object obj = SqlHelper.ExecuteScalar(dalInfo.Connection, dalInfo.LongConnection, cmdText, GetParameters());
            return obj == DBNull.Value ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 取字段值
        /// </summary>
        /// <returns></returns>
        public object QueryAttribute()
        {
            return SqlHelper.ExecuteScalar(dalInfo.Connection, dalInfo.LongConnection, GetCmdText(), GetParameters());
        }

        #endregion

        public List<string> SqlCommands
        {
            get
            {
                List<string> list = new List<string>();

                string cmd = this.GetCmdText();
                DbParameter[] rparam = this.GetParameters();
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
                return list;
            }
        }

        #endregion

        #region private

        private string GetCmdText()
        {
            if (!string.IsNullOrEmpty(customSqlCommand))
                return customSqlCommand;
            return dalInfo.Provider.GetQueryText(top, columns, typeof(T).Name, mySelect.WhereText);
        }

        private DbParameter[] GetParameters()
        {
            if (!string.IsNullOrEmpty(customSqlCommand))
                return null;
            if (mySelect.ListWhereEntity == null)
                return null;
            int count = mySelect.ListWhereEntity.Count;
            if (count > 0)
            {
                DbParameter[] rparam = new DbParameter[count];
                int n = 0;
                string paramName = "";
                foreach (WhereEntity item in mySelect.ListWhereEntity)
                {
                    if (item.RelationType == RelationTypeEnum.IsNull)
                        continue;
                    paramName = string.Format("@{0}{1}", item.ColumnName, item.ColumnIndex > 0 ? item.ColumnIndex.ToString() : ""); 
                    rparam[n] = dalInfo.Provider.CreateParameter(paramName, item.Value);
                    n++;
                }
                return rparam;
            }
            return null;
        }

        private T GetModel(DbDataReader dr, PropertyInfo[] pi, List<string> listColumn)
        {
            var model = System.Activator.CreateInstance<T>();
            foreach (PropertyInfo p in pi)
            {
                if (!listColumn.Contains(p.Name.ToLower()))
                    continue;
                if (!p.CanWrite)
                    continue;
                if (dr[p.Name] != DBNull.Value)
                {
                    if (p.PropertyType.Name.IndexOf("String") > -1)
                        p.SetValue(model, dr[p.Name].ToString().Trim(), null);
                    else if (p.PropertyType.FullName.IndexOf("Int64") > -1)
                        p.SetValue(model, Convert.ToInt64(dr[p.Name].ToString()), null);
                    else if (p.PropertyType.FullName.IndexOf("Int32") > -1)
                        p.SetValue(model, Convert.ToInt32(dr[p.Name].ToString()), null);
                    else
                        model.GetType().GetProperty(p.Name).SetValue(model, dr[p.Name], null);
                }
            }
            return model;
        }

        #endregion




    }
}
