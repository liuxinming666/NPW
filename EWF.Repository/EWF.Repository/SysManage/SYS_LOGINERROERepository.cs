/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_LOGINERROERepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository.SysManage;
using EWF.Util.Options;
using EWF.Util;
using EWF.Entity;
using System.Data;

namespace EWF.Repository
{
    public class SYS_LOGINERROERepository:DefaultRepository<SYS_LOGINERROE>, ISYS_LOGINERROERepository
    {
        public SYS_LOGINERROERepository(IOptionsSnapshot<DbOption> options):base(options)
        {
        }
        public DataTable GetLoginErrorCount(string userip, string username, DateTime tm)
        {
            string strSql = string.Format("select count(LEID)  as lcount from TBL_SYS_LOGINERROE where LOGINTIME>'{0}' and USERIP='{1}' and USERNAME='{2}'", tm, userip, username);
            return database.FindTable(strSql);            
        }
        public DataTable GetLoginByHasSQLCount(string userip, string username, DateTime tm, string ishassql = "0")
        {
            string strSql = string.Format("select count(LEID)  as lcount from TBL_SYS_LOGINERROE where LOGINTIME>'{0}' and USERIP='{1}' and ISHASSQL='{2}'", tm, userip, ishassql);
            return database.FindTable(strSql);
        }
        public bool DeleteLoginError(string userip, string username)
        {
            string strSql = string.Format("delete from TBL_SYS_LOGINERROE where USERNAME='{0}'", username);
            int obj = database.ExecuteBySql(strSql);
            if (obj > 0)
                return true;
            else
                return false;
        }

    }
}