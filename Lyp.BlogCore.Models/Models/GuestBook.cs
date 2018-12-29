using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lyp.BlogCore.Models.Models
{
    /// <summary>
    /// 留言
    /// </summary>
    public class GuestBook
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        [Key]
        public int gID { get; set; }
        /// <summary>
        /// 留言人
        /// </summary>
        public string gUserName { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string gBody { get; set; }
        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime gCreateTime { get; set; }
    }
}
