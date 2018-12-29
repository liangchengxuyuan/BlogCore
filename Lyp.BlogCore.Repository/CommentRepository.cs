using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Repository.Base;
using Lyp.BlogCore.Repository.MySqlEFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Repository
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(MySqlDbContext context) : base(context)
        {

        }
    }
}
