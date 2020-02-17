using EWF.Util;
using EWF.Util.Config;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace EWF.Data
{
    public class ConnectionFactory
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection(string strConn, DatabaseType databaseType = DatabaseType.SqlServer)
        {
            IDbConnection connection = null;
            //获取配置进行转换
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    connection = new SqlConnection(strConn);
                    break;
                case DatabaseType.Oracle:
                    connection = new OracleConnection(strConn);
                    break;
                case DatabaseType.MySQL:
                    connection = new MySqlConnection(strConn);
                    break;
                case DatabaseType.Sqlite:
                    connection = new SQLiteConnection(strConn);
                    break;
            }
            return connection;
        }
    }
}
