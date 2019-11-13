using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Reflection;

namespace HKLite
{
    public class Config
    {
        string dir="";
        string fileName="HKLITE.CFG.XML";
        string filePath = "";
        private XmlDocument xDoc = new XmlDocument();
       
        private string dbMark;
        private XmlNode dbNode;

        public Config(string dbMark, string dbDir)
        {
            this.dbMark = dbMark;

            if (!Directory.Exists(dbDir))
                Directory.CreateDirectory(dbDir);
            // dbDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.ToString());
             if (System.Environment.OSVersion.Platform.ToString().ToLower() != "wince")
                 dbDir = dbDir.Replace("file:\\", "");            
            filePath = string.Format(@"{0}\{1}", dbDir, fileName);

            if (File.Exists(filePath))
            {
                xDoc.Load(filePath);
            }
            else
            {
                xDoc.AppendChild(xDoc.CreateElement("root"));
            }
            dbNode = xDoc.SelectSingleNode(string.Format("root//{0}", dbMark));
        }

        /// <summary>
        /// 取配置文件节点值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            if (dbNode == null)
                return string.Empty;

            var obj = dbNode.SelectSingleNode("field[@name='" + name + "']");
            if (obj == null)
                return string.Empty;
            return ((XmlElement)obj).GetAttribute("value");

        }


        public void SetValue(string name, string value)
        {
            if (dbNode == null)
            {
                xDoc.ChildNodes[0].AppendChild(xDoc.CreateElement(dbMark));
                dbNode = xDoc.SelectSingleNode(string.Format("root//{0}", dbMark));
            }
            XmlElement xe = (XmlElement)dbNode.SelectSingleNode("field[@name='" + name + "']");
            if (xe != null)
            {
                xe.SetAttribute("value", value);
            }
            else
            {
                xe = xDoc.CreateElement("field");
                xe.SetAttribute("name", name);
                xe.SetAttribute("value", value);
                dbNode.AppendChild(xe);
            }
        }


        public void Save()
        {
            xDoc.Save(filePath);
        }



    }
}
