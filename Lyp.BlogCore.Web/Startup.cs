using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lyp.BlogCore.IRepository;
using Lyp.BlogCore.IServices;
using Lyp.BlogCore.Repository;
using Lyp.BlogCore.Repository.MySqlEFCore;
using Lyp.BlogCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lyp.BlogCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //var connection = Configuration.GetConnectionString("MysqlConnection");
            //services.AddDbContext<MySqlDbContext>(options => options.UseMySQL(connection));
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            //services.Add(new ServiceDescriptor(typeof(IAdminService), typeof(AdminService), ServiceLifetime.Singleton));
            var irepositoryDllFile = Path.Combine(basePath, "Lyp.BlogCore.IRepository");//获取项目绝对路径
            var assemblysIRepository = Assembly.LoadFile(irepositoryDllFile);//直接采用加载文件的方法
            var repositoryDllFile = Path.Combine(basePath, "Lyp.BlogCore.Repository");//获取项目绝对路径
            var assemblysRepository = Assembly.LoadFile(repositoryDllFile);//直接采用加载文件的方法


            services.AddTransient<IAdminRepository, AdminReposiory>();
            services.AddTransient<IAdminService, AdminService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Admins}/{action=Index}/{id?}");
            });
        }
    }
}
