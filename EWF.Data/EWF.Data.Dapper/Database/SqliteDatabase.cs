/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 18:24:40
 * 描  述：SqliteHelper
 * *********************************************/
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace EWF.Data.Dapper
{
    public class SqliteDatabase : Database
    {
        public SqliteDatabase(string connectionString) :
           base(connectionString, DatabaseType.Sqlite)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
        }
    }
}
