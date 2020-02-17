/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_ACCESSTIMERepository                                      
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

namespace EWF.Repository
{
    public class SYS_ACCESSTIMERepository: DefaultRepository<SYS_ACCESSTIME>, ISYS_ACCESSTIMERepository
    {
        public SYS_ACCESSTIMERepository(IOptionsSnapshot<DbOption> options):base(options)
        {
        }
        public bool UpdateLastAccessTime(string uid, string authority)
        {
            string strSql = string.Format("update TBL_SYS_ACCESSTIME  set loginouttime = (select max(atime) from dbo.TBL_SYS_ACCESSRECORD where uid ='{0}'  and appip ='{1}' and atype = 1), atype = '1' where uid ='{0}'  and appip ='{1}' and atype = 0", uid, authority);
            int obj = database.ExecuteBySql(strSql);
            if (obj > 0)
                return true;
            else
                return false;
        }


    }
}