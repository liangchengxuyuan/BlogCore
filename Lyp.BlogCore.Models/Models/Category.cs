using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lyp.BlogCore.Models.Models
{
    /// <summary>
    /// 文章类别
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 类别ID
        /// </summary>
        [Key]
        public int cID { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string cName { get; set; }
    }
}
