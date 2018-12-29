using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Services
{
    public class BlogArticleService : BaseServices<BlogArticle>, IBlogArticleService
    {
        IBlogArticleRepository dal;
        public BlogArticleService(IBlogArticleRepository dal)
        {
            this.dal = dal;
            this.baseDal = dal;
        }
    }
}
