using Lyp.BlogCore.IServices;
using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Services
{
    public class AdminService:BaseServices<Admin>,IAdminService
    {
        IAdminRepository dal;
        public AdminService(IAdminRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
    }
}
