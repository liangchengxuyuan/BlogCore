using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Repository
{
    public class AdminReposiory:BaseRepository<Admin>,IAdminRepository
    {
    }
}
