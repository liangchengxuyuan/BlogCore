using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Api.AOP
{
    public class UserAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var UserName = context.HttpContext.Session.GetString("UserName");
            if(string.IsNullOrEmpty(UserName))
            {
                context.Result=new RedirectResult("http://baidu.com");
            }
        }
    }
}
