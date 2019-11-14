using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HKLite;
using System.IO;
using System.Reflection;

namespace HKLiteDemo
{
    public static class DBAccess
    {
        // database version
        private static int dbVersion = 1;
        private static IDALayer dal;

        internal static IDALayer DAL
        {
            get { return dal; }
        }

        /// <summary>
        /// Init database
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = string.Format(@"{0}\db\mydb.db", dir);
            dal = DALayerBuilder.CreateLayer(dbPath, null, "HKLiteDemo.Entity.DLL", dbVersion, false);
            dal.CreatedDatabase += new OnCreatedDatabase(DatabaseCreated);
            dal.UpgradedDataBase += new OnUpdatedDataBase(UpgradedDataBase);
            dal.Init();

            return true;
        }

        /// <summary>
        /// Do after database created
        /// </summary>
        private static void DatabaseCreated()
        {
            dal.ExecuteTransac(
                "create index index_SysUser_001 on SysUser(UserName)",
                "create index index_SysRole_001 on SysRole(RoleName)"
                );
        }

        /// <summary>
        /// Do after database upgraded
        /// </summary>
        private static void UpgradedDataBase(int oldVersion, int newVersion)
        {
        }
    }
}
 