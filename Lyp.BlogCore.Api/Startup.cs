using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Blog.Core.Log;
using log4net;
using log4net.Config;
using log4net.Repository;
using Lyp.BlogCore.Repository.MySqlEFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lyp.BlogCore.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options=> {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //连接字符串注入
            var connection = Configuration.GetConnectionString("MysqlConnection");
            services.AddDbContext<MySqlDbContext>(options => options.UseMySQL(connection));

            //log日志注入
            services.AddSingleton<ILoggerHelper, LogHelper>();

            //session注入、设置过期时间
            services.AddSession(options=> {
                options.IdleTimeout = TimeSpan.FromHours(24);
            });
            #region AutoMapper注入

            services.AddAutoMapper(typeof(Startup));

            #endregion

            #region AutoFac

            var builder = new ContainerBuilder();
            //builder.RegisterType<BlogNameAOP>();

            var assemblysServices = Assembly.Load("Lyp.BlogCore.Services");
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(assemblysServices)
            //          .AsImplementedInterfaces()
            //          .InstancePerLifetimeScope()
            //          .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
            //          .InterceptedBy(typeof(BlogNameAOP));//可以直接替换拦截器

            ////将services中的服务填充到Autofac中.
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
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
