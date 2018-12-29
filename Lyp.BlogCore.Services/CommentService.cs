using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Services
{
    public class CommentService : BaseServices<Comment>, ICommentService
    {
        ICommentRepository dal;
        public CommentService(ICommentRepository dal)
        {
            this.dal = dal;
            this.baseDal = dal;
        }
    }
}
