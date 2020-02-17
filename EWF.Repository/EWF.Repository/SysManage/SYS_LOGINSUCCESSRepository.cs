/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_LOGINSUCCESSRepository                                      
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
    public class SYS_LOGINSUCCESSRepository: DefaultRepository<SYS_LOGINSUCCESS>, ISYS_LOGINSUCCESSRepository
    {
        public SYS_LOGINSUCCESSRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
		}

		public bool DeleteLoginSuccess(string userip, string username, DateTime tm)
        {
            string strSql = string.Format("delete from TBL_SYS_LOGINSUCCESS where USERNAME='{0}' and LOGINTIME<'{1}'", username, tm);
            int obj = database.ExecuteBySql(strSql);
            if (obj > 0)
                return true;
            else
                return false;
        }
        public DataTable GetLoginSuccessCount(string userip, string username, DateTime tm)
        {
            string strSql = string.Format("select count(LSID)  as lcount from TBL_SYS_LOGINSUCCESS  where LOGINTIME>'{0}' and USERIP='{1}'", tm, userip);
            return database.FindTable(strSql);
        }

    }
}