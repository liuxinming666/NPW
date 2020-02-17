/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/1/25 16:54:55
 * 描  述：DapperFactory
 * *********************************************/
using EWF.Data.Dapper;
using EWF.Util;
using EWF.Util.Config;
using EWF.Util.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Data.Repository
{
    public class DapperFactory
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDatabase CreateDapper(string dbtype, string strConn)
        {
            if (dbtype.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接，数据库类型不能为空!");
            if (strConn.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接，数据库连接字符串不能为空!");
            var dbType = GetDataBaseType(dbtype);
            return CreateDapper(dbType, strConn);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDatabase CreateDapper(DatabaseType dbType, string strConn)
        {
            IDatabase dapper = null;
            if (strConn.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接，数据库连接字符串不能为空!");

            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    dapper = new SqlServerDatabase(strConn);
                    break;
                case DatabaseType.Oracle:
                    dapper = new OracleDatabase(strConn);
                    break;
                case DatabaseType.MySQL:
                    dapper = new MySqlDatabase(strConn);
                    break;
                case DatabaseType.Sqlite:
                    dapper = new SqliteDatabase(strConn);
                    break;
                default:
                    throw new ArgumentNullException($"系统不支持的{dbType.ToString()}数据库类型");
            }
            return dapper;
        }

        public static IDatabase CreateDapper(string connName)
        {
            if (connName.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接，数据库类型不能为空!");
            var dbOption = ConfigHelper.GetValue<DbOption>(connName);
            if (dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            return CreateDapper(dbOption.DbType, dbOption.ConnectionString);
        }

        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="dbtype">数据库类型字符串</param>
        /// <returns>数据库类型</returns>
        public static DatabaseType GetDataBaseType(string dbtype)
        {
            if (dbtype.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            DatabaseType returnValue = DatabaseType.SqlServer;
            foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
            {
                if (dbType.ToString().Equals(dbtype, StringComparison.OrdinalIgnoreCase))
                {
                    returnValue = dbType;
                    break;
                }
            }
            return returnValue;
        }
    }
}
