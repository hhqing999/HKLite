using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKLiteDemo.Entity
{
    /// <summary>
    /// 用户权限表
    /// </summary>
    [HKLite.Entity]
    public class SysUserRole
    {
        [HKLite.Entity(PrimaryKey = true, Identity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }

    }
}
