using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EWF.Util.Ioc
{
    public class AutoFacTest
    {
        public void TestCode()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DBManager>();

            //builder.RegisterType<SqlDAL>().As<IDAL>();
            List<NamedParameter> ListNamedParameter = new List<NamedParameter>() { new NamedParameter("Id", 1), new NamedParameter("Name", "张三") };
            builder.RegisterType<SqlDAL>().Named<IDAL>("sql").WithParameter(new NamedParameter("configSectionName", "sectionName"));
            builder.RegisterType<OracleDAL>().Named<IDAL>("oracle");

            
           // builder.RegisterType<OracleDAL>().Keyed<IDAL>(DBType.SqlServer);
            //builder.RegisterType<OracleDAL>().Keyed<IDAL>(DBType.Oracle);


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()); //在当前正在运行的程序集中找
            //builder.RegisterType<Worker>().As<IPerson>();


            using (var container = builder.Build())
            {
                var manager = container.Resolve<DBManager>();
                manager.Add("INSERT INTO Persons VALUES ('Man', '25', 'WangW', 'Shanghai')");

                SqlDAL sqlDAL = (SqlDAL)container.ResolveNamed<IDAL>("sql");

                //SqlDAL sqlDAL1= (SqlDAL)container.ResolveKeyed<IDAL>(DBType.SqlServer);
            }
        }

        public void TestJsonConfig() {

            //注册
            // Add the configuration to the ConfigurationBuilder.
            var config = new ConfigurationBuilder();
            config.AddJsonFile("Config/JsonConfig/autofac.json");

            // Register the ConfigurationModule with Autofac.
            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            //调用
            using (var container = builder.Build())
            {
                var manager = container.Resolve<DBManager>();
                manager.Add("INSERT INTO Persons VALUES ('Man', '25', 'WangW', 'Shanghai')");

                SqlDAL sqlDAL = (SqlDAL)container.ResolveNamed<IDAL>("sql");

                //SqlDAL sqlDAL1 = (SqlDAL)container.ResolveKeyed<IDAL>(DBType.SqlServer);
            }
        }

    }



    public interface IDAL
    {
        void Insert(string commandText);
    }

    public class SqlDAL : IDAL
    {
        string connect = "";
        public SqlDAL(string conn) {
            connect = conn;
        }

        public void Insert(string commandText)
        {
            Console.WriteLine("使用sqlDAL添加相关信息" + connect);
        }
    }

    public class OracleDAL : IDAL
    {
        public void Insert(string commandText)
        {
            Console.WriteLine("使用OracleDAL添加相关信息");
        }
    }

    public class DBManager
    {
        IDAL _dal;
        public DBManager(IDAL dal)
        {
            _dal = dal;
        }
        public void Add(string commandText)
        {
            _dal.Insert(commandText);
        }
    }
}
