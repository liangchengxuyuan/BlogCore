using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Models.ViewModels
{
    public class BlogDetailVM
    {
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
        /// 类别
        /// </summary>
        public string Category { get; set; }
    }
}
