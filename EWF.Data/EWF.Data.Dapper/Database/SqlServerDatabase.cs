/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 18:11:17
 * 描  述：SqlServerHelper
 * *********************************************/
using EWF.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Data.Dapper
{
    public class SqlServerDatabase : Database
    {
        public SqlServerDatabase(string connectionString) :
            base(connectionString, DatabaseType.SqlServer)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
        }
    }
}
