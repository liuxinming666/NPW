/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2018/12/25 10:14:14                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.Test                                   
*│　类    名： Common                                      
*└──────────────────────────────────────────────────────────────┘
*/
//using Czar.Cms.Core.CodeGenerator;
//using Czar.Cms.Core.Models;
//using Czar.Cms.Core.Options;
//using Czar.Cms.Core.Repository;
//using Czar.Cms.IRepository;
//using Czar.Cms.Repository.SqlServer;
using EWF.Util;
using EWF.Util.CodeGenerator;
using EWF.Util.CodeGenerator.Options;
using EWF.Util.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.Test
{
    public class Common
    {
        /// <summary>
        /// 构造依赖注入容器，然后传入参数
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider BuildServiceForSqlServer()
        {
            var services = new ServiceCollection();
            services.Configure<CodeGenerateOption>(options =>
            {
                options.ConnectionString = "Data Source=192.168.1.88;Initial Catalog=EW_NPW;User ID=sa;Password=123;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                options.DbType = DatabaseType.SqlServer.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType

                //options.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.168.1.88)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl88)));user id=rtdb;password=digital1;Incr Pool Size=5;Decr Pool Size=2;";
                //options.DbType = DatabaseType.Oracle.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType

                options.Author = "zhujun";//作者名称
                options.OutputPath = "E:\\company\\EW_NPW\\EW_NPWCodeGenerator";//模板代码生成的路径
                options.ModelsNamespace = "EWF.Entity";//实体命名空间
                //options.MappingNamespace="EWF.Mapping";//实体映射命名空间
                options.IRepositoryNamespace = "EWF.IRepository";//仓储接口命名空间
                options.RepositoryNamespace = "EWF.Repository";//仓储命名空间
                options.IServicesNamespace = "EWF.IServices";//服务接口命名空间
                options.ServicesNamespace = "EWF.Services";//服务命名空间
                options.ConnName = "Default_Option";//默认数据库连接名称

            });
            services.Configure<DbOption>("EWF", GetConfiguration().GetSection("Default_Option"));
            //services.AddScoped<IArticleRepository, ArticleRepository>();
            //services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<EWF.Util.CodeGenerator.CodeGeneratorBuilder>();
            return services.BuildServiceProvider(); //构建服务提供程序
        }

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
