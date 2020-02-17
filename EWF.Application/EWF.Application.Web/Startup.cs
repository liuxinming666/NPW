using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EWF.Util;
using System.Reflection;
using EWF.Util.Options;
using Microsoft.Extensions.Logging;
using EWF.Util.Log;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EWF.Application.Web
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.LoginPath = "/Access/Login";
               options.LogoutPath = "/Access/Login";
               options.ExpireTimeSpan = TimeSpan.FromMinutes(120);//.FromHours(2);
           });
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(15);
            //    options.Cookie.HttpOnly = true;
            //});
            //注入全局异常捕获
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptions));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
           

            services.AddOptions();
            services.Configure<DbOption>("Default_Option", Configuration.GetSection("Default_Option"));
            services.Configure<DbOption>("File_Opion", Configuration.GetSection("File_Opion"));
            services.Configure<DbOption>("RWDB_Opion", Configuration.GetSection("RWDB_Opion"));
            services.Configure<DbOption>("RTDB_Opion", Configuration.GetSection("RTDB_Opion"));
            services.Configure<DbOption>("newmanage_Opion", Configuration.GetSection("newmanage_Opion"));
            //气象路径
            services.Configure<WeaterConfig>("YbWeather", Configuration.GetSection("YbWeather")); 
            services.Configure<DataOption>("DataOption", Configuration.GetSection("MyDataOption")); 
            //(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.168.1.88)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl88)))


            //批量注册服务
            services.BatchRegisterService(new Assembly[] {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("EWF.IRepository"),
                //Assembly.Load("EWF.Repository.Oracle"),
                Assembly.Load("EWF.Repository"),
                Assembly.Load("EWF.IServices"),
                Assembly.Load("EWF.Services")}, typeof(IDependency), ServiceLifetime.Transient);

            //注册自定义日志服务
            services.AddTransient<LoggerHelper>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //加载log4net中间件，替换默认的日志组件
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseSession();
            app.UseAuthentication();

            #region Route路由
            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    name: "RealData",
                    areaName: "RealData",
                    template: "RealData/{controller=Home}/{action=Index}/{id?}" );
                routes.MapAreaRoute(
                    name: "StationInfo",
                    areaName: "StationInfo",
                    template: "StationInfo/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaRoute(
                    name: "HistoryInfo",
                    areaName: "HistoryInfo",
                    template: "HistoryInfo/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaRoute(
                    name: "MapVideo",
                    areaName: "MapVideo",
                    template: "MapVideo/{controller=Home}/{action=Index}/{id?}" );
               routes.MapAreaRoute(
                    name: "SysManage",
                    areaName: "SysManage",
                    template: "SysManage/{controller=Home}/{action=Index}/{id?}" );
               routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    //template: "{controller=Home}/{action=Index}/{id?}");
                    template: "{controller=Access}/{action=Login}/{id?}");


            });

            #endregion
            
        }
    }
}
