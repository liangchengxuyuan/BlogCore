using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lyp.BlogCore.Models.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        public int uID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string uUserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string uPassWord { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        [Column(TypeName = "bit")]
        public bool uIsAdministrators { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime uCreateTime { get; set; }
    }
}
