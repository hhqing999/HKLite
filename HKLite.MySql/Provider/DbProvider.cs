using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Reflection ;

namespace HKLite
{
    internal sealed class DbProvider : IDbProvider
    {
        private string errorMsg = "该方法不支持当前数据库类型";

        public DbConnection CreateConnection()
        {
            return new MySqlConnection(); 
        }

        public DbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public string GetConnectionString(string dbPath, string password)
        {
            throw new Exception(errorMsg);
        }
        
        public DbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        public DbParameter CreateParameter(string paramName, object paramValue)
        {
            return new MySqlParameter(paramName, paramValue);
        }

        public DbParameter CreateParameter()
        {
            return new MySqlParameter();
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        public void CreateDatabase(string dbPath, string password)
        {
            throw new Exception(errorMsg);
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
            int n = 0;
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

                sb.Append(GetColumnType(pinfo, dataType, length));
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

                if (identity)
                    sb.Append(" AUTO_INCREMENT");

                n++;
            }
            sb.Append("\r\n");
            if (keyName != null && keyName.Trim().Length > 0)
            {
                sb.Append(",");
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
            string sqlCheck = string.Format("SELECT * FROM {0} LIMIT 1;", tableName);
            try
            {
                dt = SqlHelper.ExecuteDataTable(dalInfo.Connection, dalInfo.Provider.CreateDataAdapter(), dalInfo.LongConnection, sqlCheck, null);
            }
            catch (Exception ex)
            { }
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

            bool primaryKey;
            bool identity;
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
                primaryKey = false;
                identity = false;
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
                        primaryKey = true;
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

                sb.Append("alter table ");
                sb.Append(tableName);
                sb.Append(" add ");
                sb.Append(pinfo.Name);
                sb.Append(" ");
                sb.Append(GetColumnType(pinfo, dataType, length));
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

                if (identity)
                    sb.Append(" AUTO_INCREMENT");
                list.Add(sb.ToString());
                if (primaryKey)
                {
                    sb.Remove(0, sb.Length);
                    sb.Append("alter table ");
                    sb.Append(tableName);
                    sb.Append(" add constraint");
                    sb.Append(" pk_");
                    sb.Append(tableName);
                    sb.Append("_");
                    sb.Append(pinfo.Name);
                    sb.Append(" primary key(");
                    sb.Append(pinfo.Name);
                    sb.Append(")");
                    list.Add(sb.ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetColumnType(PropertyInfo p, string dataType, object length)
        {
            string value = "";
            if (dataType != null && dataType.Trim().Length > 0)
            {
                value = dataType;
                if (dataType.Trim().ToLower() != "text" && p.PropertyType.FullName.IndexOf("String") > -1)
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
                value = "varchar";
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
                value = "bool";
            }
            else if (p.PropertyType.FullName.IndexOf("Int64") > -1)
            {
                value = "bigint";
            }
            else if (p.PropertyType.FullName.IndexOf("Int32") > -1)
            {
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
                sql = string.Format("{0} LIMIT {1}", sql, top);
            }
            sql = string.Format("{0};", sql);
            return sql;
        }


    }
}
