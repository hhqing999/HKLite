using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKLiteDemo.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [HKLite.Entity]
    public class SysRole
    {
        [HKLite.Entity(PrimaryKey = true, Identity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [HKLite.Entity(Length = "50")]
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [HKLite.Entity(Length = "50")]
        public string Description { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
    }
}
