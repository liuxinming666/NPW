/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 18:23:09
 * 描  述：MySqlHelper
 * *********************************************/
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace EWF.Data.Dapper
{
    public class MySqlDatabase : Database
    {
        public MySqlDatabase(string connectionString) :
           base(connectionString, DatabaseType.MySQL)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
        }
    }
}
