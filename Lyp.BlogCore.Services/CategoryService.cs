using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Services
{
    public class CategoryService : BaseServices<Category>, ICategoryService
    {
        ICategoryRepository dal;
        public CategoryService(ICategoryRepository dal)
        {
            this.dal = dal;
            this.baseDal = dal;
        }
    }
}
