using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Repository.Base;
using Lyp.BlogCore.Repository.MySqlEFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MySqlDbContext context) : base(context)
        {

        }
    }
}
