using AutoMapper;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Web.AutoMapper
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminViewModel>();
        }
    }
}
