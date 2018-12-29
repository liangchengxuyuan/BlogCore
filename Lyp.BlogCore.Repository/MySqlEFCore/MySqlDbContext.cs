using Lyp.BlogCore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Repository.MySqlEFCore
{
    public class MySqlDbContext:DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //注入Sql链接字符串
        //    //optionsBuilder.UseMySQL(@"Data Source=127.0.0.1;Database=BlogDB;User ID=root;Password=lyp123;pooling=true;port=3306;sslmode=none;CharSet=utf8;");
        //}
        public DbSet<Admin> admins { get; set; }
        public DbSet<BlogArticle> blogArticles { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<GuestBook> guestBooks { get; set; }
        public DbSet<UserInfo> userInfos { get; set; }
    }
}
