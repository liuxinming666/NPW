using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace EWF.Util.Config
{
    public class ConfigCommon
    {
        public  IConfigurationRoot LoadConfigurationBase()
        {
            var Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile("appsettings.Development.json", true, true)
               .Build();

            return Configuration;
        }

        public  IConfigurationRoot LoadJsonConfiguration(string fileName)
        {
            var Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(fileName, true, true)
               .Build();
            return Configuration;
        }

        public  IConfigurationRoot LoadXMLConfiguration(string fileName)
        {
            var Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddXmlFile(fileName, true, true)
               .Build();
            return Configuration;
        }

        public  Configuration LoadXMLConfiguration_Old(string fileName)
        {
            //根据文件名获取指定config文件
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = fileName };

            //从config文件中读取配置信息
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return configuration;
        }

        public  T GetValue<T>(IConfiguration configuration, string key) where T : class, new()
        {
            var appconfig = new ServiceCollection()
              .AddOptions()
              .Configure<T>(configuration.GetSection(key))
              .BuildServiceProvider()
              .GetService<IOptions<T>>();
            return appconfig.Value;
        }

        public  string GetValue(IConfiguration configuration, string key)
        {
            //var dd = Configuration.GetSection("Logging")["IncludeScopes"];
            //var dd1 = Configuration["Logging:Model:Name"];
            //int dd2 = Configuration.GetValue<int>("Logging:Model:Age");//string转int

            return configuration[key];
        }
    }
}
