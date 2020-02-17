using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.Ioc
{
    public class AutoFacIocHelper
    {
        private IContainer _container;
        private static AutoFacIocHelper dbinstance;

        private AutoFacIocHelper()
        {
            LoadConfig();
            if (dbinstance == null)
            {
                dbinstance = new AutoFacIocHelper();
            }
        }

        public static AutoFacIocHelper DBInstance
        {
            get { return dbinstance; }
        }

        #region 调用：获取类型

        public T GetService<T>()
        {
            return _container.Resolve<T>();
        }
        public T GetServiceNamed<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }
        public T GetServiceKeyed<T>(object serviceKey)
        {
            return _container.ResolveKeyed<T>(serviceKey);
        }
        #endregion

        #region 加载配置并初始化容器
        private void LoadConfig() {
            //注册
            // Add the configuration to the ConfigurationBuilder.
            var config = new ConfigurationBuilder();
            config.AddJsonFile("Config/JsonConfig/autofac.json");

            // Register the ConfigurationModule with Autofac.
            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            
            _container = builder.Build();
        }

        private void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DBManager>();

            //builder.RegisterType<SqlDAL>().As<IDAL>();
            builder.RegisterType<SqlDAL>().Named<IDAL>("sql");
            builder.RegisterType<OracleDAL>().Named<IDAL>("oracle");

          
        }
        #endregion

    }
}
