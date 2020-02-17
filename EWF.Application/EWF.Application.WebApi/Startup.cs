using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EWF.Util;
using EWF.Util.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EWF.Application.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            //日志加载配置
            //LogHelper.LoadConfig();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOptions();
            services.Configure<DbOption>("Default_Opion", Configuration.GetSection("Default_Option"));
            services.Configure<DbOption>("File_Opion", Configuration.GetSection("File_Opion"));
            services.Configure<DbOption>("RWDB_Opion", Configuration.GetSection("RWDB_Opion"));


            //注册服务
            //services.AddTransient<IMenuService, MenuService>();
            //批量注册服务
            services.BatchRegisterService(new Assembly[] {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("EWF.IRepository"),
                Assembly.Load("EWF.Repository"),
                Assembly.Load("EWF.IServices"),
                Assembly.Load("EWF.Services")}, typeof(IDependency), ServiceLifetime.Transient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");


            });
        }
    }
}
