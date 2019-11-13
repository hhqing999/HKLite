using System;

using System.Collections.Generic;
using System.Text;

using System.Data.Common;
using System.Data;
using System.Reflection;

namespace HKLite
{
    internal class DBHelper
    {
        public bool CreateTables(DalLayerInfo dalInfo, Type[] entityTypes)
        {
            List<string> sqls = new List<string>();
            object[] customAttributes;
            Type attType = typeof(EntityAttribute);
            foreach (Type item in entityTypes)
            {
                bool isEntity = false;
                customAttributes = item.GetCustomAttributes(false);
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

                sqls.Add(dalInfo.Provider.GetTableScript(item));
            }

            return SqlHelper.ExecuteTransac(dalInfo.Connection, dalInfo.LongConnection, sqls);
        }

        public void SyncTables(ITransacBuilder transacBuilder, DalLayerInfo dalInfo, Type[] entityTypes)
        {
            List<string> sqls = new List<string>();
            
            object[] customAttributes;
            Type attType = typeof(EntityAttribute);
            foreach (Type item in entityTypes)
            {
                bool isEntity = false;
                customAttributes = item.GetCustomAttributes(false);
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
                List<string> tempSql = dalInfo.Provider.GetNewColumnsCommandTexts(dalInfo, item);
                if (tempSql != null && tempSql.Count > 0)
                    sqls.AddRange(tempSql);
            }
            foreach (var item in sqls)
            {
                transacBuilder.AddCommandText(item);
            }
        }

    }
}
