using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKLiteDemo.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [HKLite.Entity]
    public class SysUser
    {
        [HKLite.Entity(PrimaryKey=true,Identity=true)]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [HKLite.Entity(Length="20")]
        public string UserName { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [HKLite.Entity(Length = "50")]
        public string Email { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [HKLite.Entity(Length = "50")]
        public string CName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
    }
}
