using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Services
{
    public class GuestBookService : BaseServices<GuestBook>, IGuestBookService
    {
        IGuestBookRepository dal;
        public GuestBookService(IGuestBookRepository dal)
        {
            this.dal = dal;
            this.baseDal = dal;
        }
    }
}
