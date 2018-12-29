using AutoMapper;
using Lyp.BlogCore.Models.Models;
using Lyp.BlogCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Api.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<BlogArticle, BlogDetailVM>();
        }
    }
}
