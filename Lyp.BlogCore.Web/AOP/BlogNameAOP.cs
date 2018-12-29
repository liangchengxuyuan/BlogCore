using Castle.DynamicProxy;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Web.AOP
{
    public class BlogNameAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //var admin =  adminService.QueryWhere(s => s.ID == 1);
            //admin.FirstOrDefault().UserName = "测试一下aop";
            //adminService.Update(admin.FirstOrDefault());
            
            invocation.Proceed();
        }
    }
}
