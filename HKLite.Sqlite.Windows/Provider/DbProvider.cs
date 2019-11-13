using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection ;

namespace HKLite
{
    internal sealed class DbProvider : IDbProvider
    {
        public DbConnection CreateConnection()
        {
            return new SQLiteConnection(); 
        }

        public DbConnection CreateConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        public string GetConnectionString(string dbPath, string password)
        {
            if(password ==null ||password .Trim ().Length ==0)
                return string.Format("Data Source={0};;Pooling=true", dbPath);
            return string.Format("Data Source={0};Password={1};;Pooling=true;", dbPath, password);
        }
        
        public DbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        public DbParameter CreateParameter(string paramName, object paramValue)
        {
            return new SQLiteParameter(paramName, paramValue);
        }

        public DbParameter CreateParameter()
        {
            return new SQLiteParameter();
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter();
        }

        public void CreateDatabase(string dbPath, string password)
        {
            string dbdir = Common.GetDir(dbPath);
            if (System.IO.Directory.Exists(dbdir) == false)
                System.IO.Directory.CreateDirectory(dbdir);
            SQLiteConnection.CreateFile(dbPath);

            if (password != null && password.Trim().Length > 0)
            {
                var con = new SQLiteConnection(string.Format("Data Source={0}", dbPath));
                con.Open();
                con.ChangePassword(password);
                con.Close();
                con.Dispose();
            }
        }

        public string GetTableScript(Type entityType)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("create table ");
            sb.Append(entityType.Name);
            sb.Append("(");

            string keyName = null;

            bool identity;
            object length;
            string dataType;
            bool arrowNull;
            object[] objs;

            PropertyInfo[] pinfos = entityType.GetProperties();        
            int count = entityType.GetProperties().Length;
            int n=0;
            foreach (PropertyInfo pinfo in pinfos)
            {
                identity = false;
                length = null;
                dataType = null;
                arrowNull = true;
                              
                objs = pinfo.GetCustomAttributes(false);
                if (objs != null && objs.Length > 0)
                {
                    EntityAttribute cusatt = (EntityAttribute)objs[0];
                    if (cusatt.DisplayOnly)
                    {
                        count--;
                        continue;
                    }
                    if (cusatt.PrimaryKey)
                    {
                        keyName = pinfo.Name;
                        arrowNull = false;
                    }
                    if (cusatt.Identity)
                    {
                        identity = true;
                        arrowNull = false;
                    }
                    length = cusatt.Length;
                    dataType = cusatt.DataType;
                }
                if (n > 0)
                {
                    sb.Append(",");
                }

                sb.Append("\r\n");
                sb.Append(pinfo.Name);
                sb.Append(" ");

                sb.Append(GetColumnType(pinfo, dataType, length, identity));
                if (arrowNull)
                {
                    if (pinfo.PropertyType.IsGenericType)
                        if (pinfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                            arrowNull = false;
                }
                if (!arrowNull)
                {
                    sb.Append(" not null");
                }
                else
                {
                    sb.Append(" null");
                }

                // 如果当前字段是自增字段，则必须同时是主键，否则抛出异常
                if (identity)
                {
                    if (keyName != pinfo.Name)
                        throw new Exception(string.Format("字段{0}已被设置为自增字段，必须将其设置为主键", pinfo.Name));
                    sb.Append(" PRIMARY KEY AUTOINCREMENT");
                    keyName = null;
                }

                n++;
            }
           
            if (keyName != null && keyName.Trim().Length > 0)
            {
                sb.Append(",");
                sb.Append("\r\n");
                sb.Append("primary key(");
                sb.Append(keyName);
                sb.Append(")");
            }
            sb.Append(");");
            sb.Append("\r\n");
            sb.Append("\r\n");

            return sb.ToString();
        }

        public List<string> GetNewColumnsCommandTexts(DalLayerInfo dalInfo, Type entityType)
        {
            List<string> list = new List<string>();

            string tableName = entityType.Name;
            DataTable dt = null;
            // 检查表是否存在
            string sqlCheck = string.Format("SELECT * FROM {0} LIMIT 0,1;", tableName);
            try
            {
                dt = SqlHelper.ExecuteDataTable(dalInfo.Connection, dalInfo.Provider.CreateDataAdapter(), dalInfo.LongConnection, sqlCheck, null);
            }
            catch { }
            if (dt == null)
            {
                // 表不存在
                list.Add(GetTableScript(entityType));
                return list;
            }

            // 如果表已存在，则检查字段是否完整(通过字段名称判断)
            List<string> dtColumns = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                dtColumns.Add(column.ColumnName.ToLower().Trim());
            }

            object length;
            string dataType;
            bool arrowNull;
            object[] objs;

            PropertyInfo[] pinfos = entityType.GetProperties();

            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo pinfo in pinfos)
            {
                if (dtColumns.Contains(pinfo.Name.ToLower().Trim()))
                    continue;

                sb.Remove(0, sb.Length);
                 length = null;
                dataType = null;
                arrowNull = true;

                objs = pinfo.GetCustomAttributes(false);
                if (objs != null && objs.Length > 0)
                {
                    EntityAttribute cusatt = (EntityAttribute)objs[0];
                    if (cusatt.DisplayOnly)
                        continue;
                    if (cusatt.PrimaryKey)
                    {
                        throw new Exception(string.Format("不能添加主键列(表：{0}，列：{1})", tableName, pinfo.Name));                       
                    }
                    if (cusatt.Identity)
                    {
                        throw new Exception(string.Format("不能添加自增列(表：{0}，列：{1})", tableName, pinfo.Name));
                    }
                    length = cusatt.Length;
                    dataType = cusatt.DataType;
                }

                sb.Append("alter table ");
                sb.Append(tableName);
                sb.Append(" add ");
                sb.Append(pinfo.Name);
                sb.Append(" ");
                sb.Append(GetColumnType(pinfo, dataType, length,false));
                if (arrowNull)
                {
                    if (pinfo.PropertyType.IsGenericType)
                        if (pinfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                            arrowNull = false;
                }
                if (!arrowNull)
                {
                    sb.Append(" not null");
                    sb.Append(" default 0");
                }
                else
                {
                    sb.Append(" null");
                }
                list.Add(sb.ToString());
               
            }
            return list;
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetColumnType(PropertyInfo p, string dataType, object length,bool identity)
        {
            string value = "";
            if (dataType != null && dataType.Trim().Length > 0)
            {
                value = dataType;
                if (dataType.Trim().ToLower() != "ntext" && p.PropertyType.FullName.IndexOf("String") > -1)
                {
                    if (length != null)
                        value = string.Format("{0}({1})", value, length.ToString());
                    else
                        value = string.Format("{0}({1})", value, "50");
                }
                else if (p.PropertyType.FullName.IndexOf("Decimal") > -1)
                {
                    if (length != null)
                        value = string.Format("{0}({1})", value, length.ToString());
                    else
                        value = string.Format("{0}({1})", value, "9,1");
                }
                return value;
            }

            if (p.PropertyType.FullName.IndexOf("String") > -1)
            {
                value = "nvarchar";
                if (length != null)
                    value = string.Format("{0}({1})", value, length.ToString());
                else
                    value = string.Format("{0}({1})", value, "50");
            }
            else if (p.PropertyType.FullName.IndexOf("Time") > -1)
            {
                value = "datetime";
            }
            else if (p.PropertyType.FullName.IndexOf("Decimal") > -1)
            {
                value = "numeric";
                if (length != null)
                    value = string.Format("{0}({1})", value, length.ToString());
                else
                    value = string.Format("{0}({1})", value, "9,1");
            }
            else if (p.PropertyType.FullName.IndexOf("Bool") > -1)
            {
                value = "bit";
            }
            else if (p.PropertyType.FullName.IndexOf("Int64") > -1)
            {
                if (identity)
                    value = "integer";
                else
                    value = "bigint";
            }
            else if (p.PropertyType.FullName.IndexOf("Int32") > -1)
            {
                if (identity)
                    value = "integer";
                else
                    value = "int";
            }
            else
            {
                throw new Exception(string.Format("无法识别当前字段类型：{0}，请设置当前属性的数据类型", p.Name));
            }

            return value;
        }

        public string GetQueryText(int top, string[] columns, string tableName, string where)
        {
            string sql = "SELECT";           
            if (columns == null || columns.Length == 0)
            {
                sql = string.Format("{0} *", sql);
            }
            else
            {
                int n = 0;
                foreach (string item in columns)
                {
                    if (item.Trim().Length > 0)
                    {
                        if (n == 0)
                            sql = string.Format("{0} {1}", sql, item);
                        else if (n > 0)
                            sql = string.Format("{0},{1}", sql, item);
                        n++;
                    }
                }
            }

            sql = string.Format("{0} FROM {1} {2}", sql, tableName, where);
           
            if (top > 0)
            {
                sql = string.Format("{0} LIMIT 0,{1}", sql, top);
            }
            sql = string.Format("{0};", sql);
            return sql;
        }


    }
}
