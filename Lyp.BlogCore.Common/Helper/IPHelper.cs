using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyp.BlogCore.Common.Helper
{
    public static class IPHelper
    {
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
        public static string GetClientUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }

            return ip;
        }
    }
}
