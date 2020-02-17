/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_BACKUSERRepository                                      
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
    public class SYS_BACKUSERRepository:DefaultRepository<SYS_BACKUSER>, ISYS_BACKUSERRepository
    {
        public SYS_BACKUSERRepository(IOptionsSnapshot<DbOption> options):base(options)
        {
          
        }
        public dynamic GetBackUser(string ip)
        {
            string strSql = "SELECT [BUID],[USERIP],[USERNAME],[BULAYER],[BUDATE]  FROM [dbo].[TBL_SYS_BACKUSER] WHERE [USERIP]=@userIP";
            var sqlParams = new DynamicParameters();
            sqlParams.Add("userIP", ip);
            using (var db = database.Connection)
            {
                return db.QuerySingleOrDefault<dynamic>(strSql, sqlParams);
            }
        }


    }
}