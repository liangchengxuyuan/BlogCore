using Lyp.BlogCore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Repository.MySqlEFCore
{
    public class MySqlDbContext:DbContext
    {
        //public MySqlDbContext(DbContextOptions<MySqlDbContext> options):base(options)
        //{

        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //注入Sql链接字符串
            optionsBuilder.UseMySQL(@"Data Source=192.168.112.128;Database=TestDb;User ID=root;Password=lyp123;pooling=true;port=3306;sslmode=none;CharSet=utf8;");
        }
        public DbSet<Admin> admins { get; set; }
    }
}
