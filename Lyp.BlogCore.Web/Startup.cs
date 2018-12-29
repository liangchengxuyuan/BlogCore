using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lyp.BlogCore.Web.AOP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using log4net.Repository;
using log4net;
using log4net.Config;
using Blog.Core.Log;

namespace Lyp.BlogCore.Web
{
    public class Startup
    {
        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //log4net
            repository = LogManager.CreateRepository("Lyp.BlogCore.Web");
            //指定配置文件
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
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
            //三种注入方式    AddTransient每次请求时创建对象   AddScoped每次请求时被创建，生命周期横贯整次请求    AddSingleton第一次请求时创建，之后沿用该对象，类似于单例
            //services.AddScoped<IAdminRepository, AdminReposiory>();
            //services.AddScoped<IAdminService, AdminService>();
            #region AutoMapper注入

            services.AddAutoMapper(typeof(Startup));

            #endregion

            //log日志注入
            services.AddSingleton<ILoggerHelper, LogHelper>();
            #region autofac

            var builder = new ContainerBuilder();
            builder.RegisterType<BlogNameAOP>();
            //builder.RegisterType<MySqlDbContext>();
            
            var assemblysServices = Assembly.Load("Lyp.BlogCore.Services");
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(typeof(BlogNameAOP));//可以直接替换拦截器
            builder.Populate(services);

            var assemblyRepository = Assembly.Load("Lyp.BlogCore.Repository");
            builder.RegisterAssemblyTypes(assemblyRepository).AsImplementedInterfaces();

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器

            #endregion
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
