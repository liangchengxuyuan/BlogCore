using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lyp.BlogCore.Models.Models
{
    /// <summary>
    /// 文章评论
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        [Key]
        public int cmID { get; set; }
        /// <summary>
        /// 评论人
        /// </summary>
        public string cmCommentator { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string cmCommentBody { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime cmCreateTime { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public int bID { get; set; }
    }
}
