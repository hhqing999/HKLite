using System;

using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

namespace HKLite
{
    internal class Common
    {
        /// <summary>
        /// 取WHERE 脚本
        /// </summary>
        /// <param name="myWhere"></param>
        /// <returns></returns>
        public static string GetWhereText(Where_ myWhere)
        {
            if (myWhere == null)
                return "";
            if (myWhere.CustomWhere != null && myWhere.CustomWhere.Trim().Length > 0)
                return " " + myWhere.CustomWhere;

            // 当写为 .Where();，即Where()后面没有条件时，抛出异常
            if (myWhere.ListWhereEntity.Count == 0 &&
                (myWhere.MyOrderBy == null)
                )
                throw new Exception("Where后面的条件不能为空");

            StringBuilder sbWhereText = new StringBuilder();
            foreach (WhereEntity item in myWhere.ListWhereEntity)
            {
                // 判断字段名是否为空
                if (item.ColumnName == null || item.ColumnName.Trim().Length == 0)
                    throw new Exception(string.Format("Where后面字段名不能为空\r\n{0}", sbWhereText.ToString()));

                // 判断值是否为null
                if (item.RelationType != RelationTypeEnum.IsNull && item.Value == null)
                    throw new Exception(string.Format("参数值不能为null,若查询值为null的字段，请使用IsNull(columnName)：{0}\r\n{1}", item.ColumnName, sbWhereText.ToString()));

                // 条件类型
                if (item.ConditionType == ConditionTypeEnum.None)
                    sbWhereText.Append("WHERE ");
                else if (item.ConditionType == ConditionTypeEnum.And)
                    sbWhereText.Append(" AND ");
                else if (item.ConditionType == ConditionTypeEnum.Or)
                    sbWhereText.Append(" OR ");

                // 字段名
                sbWhereText.Append(item.ColumnName);

                // 参数关系类型
                if (item.RelationType == RelationTypeEnum.Equal)
                    sbWhereText.Append(" = ");
                else if (item.RelationType == RelationTypeEnum.EqualOrLarger)
                    sbWhereText.Append(" >= ");
                else if (item.RelationType == RelationTypeEnum.Larger)
                    sbWhereText.Append(" > ");
                else if (item.RelationType == RelationTypeEnum.EqualOrSmaller)
                    sbWhereText.Append(" <= ");
                else if (item.RelationType == RelationTypeEnum.Smaller)
                    sbWhereText.Append(" < ");
                else if (item.RelationType == RelationTypeEnum.IsNull)
                    sbWhereText.Append(" IS NULL ");
                else if (item.RelationType == RelationTypeEnum.Like)
                    sbWhereText.Append(" LIKE '%" + item.Value.ToString() + "%'");
                else if (item.RelationType == RelationTypeEnum.LikeLeft)
                    sbWhereText.Append(" LIKE '%" + item.Value.ToString() + "'");
                else if (item.RelationType == RelationTypeEnum.LikeRight)
                    sbWhereText.Append(" LIKE '" + item.Value.ToString() + "%'");


                // 参数符号
                if (item.RelationType != RelationTypeEnum.IsNull
                    && item.RelationType != RelationTypeEnum.Like
                    && item.RelationType != RelationTypeEnum.LikeLeft
                    && item.RelationType != RelationTypeEnum.LikeRight
                    )
                {
                    sbWhereText.Append("@");
                    sbWhereText.Append(item.ColumnName);                    
                    if (item.ColumnIndex > 0)
                        sbWhereText.Append(item.ColumnIndex.ToString());
                }
            }
            return sbWhereText.ToString();
        }

        public static string GetOrderText(OrderBy_ myOrderBy)
        {
            if (myOrderBy == null)
                return "";
            return myOrderBy.GetText();
        }

        /// <summary>
        /// 从文件路径中取文件名
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        /// <param name="ext">是否取扩展名</param>
        /// <returns></returns>
        public static string GetFileName(string fullPath, bool ext)
        {
            try
            {
                string fileName = "";
                int len = fullPath.LastIndexOf(@"\") + 1;
                fileName = fullPath.Substring(len, fullPath.Length - len);
                if (ext == false)
                {
                    if (fileName.LastIndexOf(".") > 0)
                    {
                        fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                    }
                }
                return fileName.Trim();
            }
            catch
            {
                return "";
            }
        }

        public static string GetDir(string fullPath)
        {
            try
            {
                string dir = "";
                int len = fullPath.LastIndexOf(@"\") + 1;
                dir = fullPath.Substring(0, len);
                return dir;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 取文件MD5值
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMd5(string content)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(content);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToUpper();
        }

       


    }
}
