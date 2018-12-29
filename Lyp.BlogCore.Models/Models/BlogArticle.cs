using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lyp.BlogCore.Models.Models
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class BlogArticle
    {
        /// <summary>
        /// 博客ID
        /// </summary>
        [Key]
        public int bID { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string bAuthor { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string bTitle { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string bContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime bCreateTime { get; set; }
        /// <summary>
        /// 阅读数
        /// </summary>
        public int bReadNum { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int bCommentNum { get; set; }
        /// <summary>
        /// 类别ID
        /// </summary>
        public int cID { get; set; }
    }
}
