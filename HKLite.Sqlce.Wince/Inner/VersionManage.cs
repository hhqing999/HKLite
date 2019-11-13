using System;

using System.Collections.Generic;
using System.Text;

using System.IO ;
using System.Reflection ;
using System.Xml;

namespace HKLite
{
    internal static class VersionManage
    {
        public static int GetVersion(string dbMark,string dbDir)
        {
            Config config = new Config(dbMark,dbDir);
            string value = config.GetValue("version");
            if (value == null || value.Trim().Length == 0)
                return 0;
            return Convert.ToInt32(value);
        }

        public static void SetVersion(string dbMark, string dbDir, int dbVersion)
        {
            Config config = new Config(dbMark,dbDir);
            config.SetValue("version", dbVersion.ToString());
            config.Save();
        }

    }
}
