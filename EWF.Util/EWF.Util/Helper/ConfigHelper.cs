using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace EWF.Util
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {
        static ConfigHelper()
        {
            IConfiguration config = AutofacHelper.GetService<IConfiguration>();
            if (config == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("Config/JsonConfig/Database.json", true, true);//加载数据库默认配置文件

                config = builder.Build();
            }

            _config = config;
        }

        private static IConfiguration _config { get; }

        public static T GetValue<T>(string key) where T : class, new()
        {
            var appconfig = new ServiceCollection()
              .AddOptions()
              .Configure<T>(_config.GetSection(key))
              .BuildServiceProvider()
              .GetService<IOptions<T>>();
            return appconfig.Value;
        }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return _config[key];
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="nameOfCon">连接字符串名</param>
        /// <returns></returns>
        public static string GetConnectionString(string nameOfCon)
        {
            return _config.GetConnectionString(nameOfCon);
        }
    }
}
