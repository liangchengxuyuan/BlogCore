using Lyp.BlogCore.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Models.ViewModels
{
    public class BlogListVM
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int pageCount { get; set; }
        /// <summary>
        /// 博客列表
        /// </summary>
        public List<BlogArticle> data { get; set; }
    }
}
