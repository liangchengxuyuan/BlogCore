using Lyp.BlogCore.Common.Helper;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lyp.BlogCore.Common.Redis
{
    public class RedisManager
    {
        private static ConnectionMultiplexer instance;
        

        private static string conn = ConfigHelper.GetSectionValue("connRedis");
        private static readonly object locker = new object();

        private RedisManager()
        {
            
        }
        /// <summary>
        /// 单例获取连接
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = ConnectionMultiplexer.Connect(conn);
                    }
                }
                return instance;
            }
        }
    }
}
