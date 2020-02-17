/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-03 12:07:55                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_ROLERepository                                      
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
using EWF.Util.Page;
using System.Linq;
using EWF.Util.Log;

namespace EWF.Repository
{
    public class SYS_ROLERepository:DefaultRepository<SYS_ROLE>, ISYS_ROLERepository
    {
        private LoggerHelper logger;
        public SYS_ROLERepository(IOptionsSnapshot<DbOption> options, LoggerHelper _logger):base(options)
        {
            logger = _logger;
        }

        public Page<SYS_ROLE> GetUserData(int pageIndex, int pageSize, string rname, string addvcd, int type)
        {
            var where = " where 1=1";
            if (!addvcd.IsEmpty())
            {
                where += " and addvcd='" + addvcd + "' and type=" + type; 
            }
            //where = " where addvcd='" + addvcd + "' and type=" + type;
            var orderby = "ID";
            var sqlParams = new DynamicParameters();            
            if (!rname.IsEmpty())
            {
                where += " and (RoleName like '%" + rname + "%')";
                //sqlParams.Add("STCDS", rname);
            }
            
            var page = GetListPaged(pageIndex, pageSize, where, orderby);
            return page;
        }

        public IEnumerable<SYS_ROLEMENUMAP> GetRoleMenuCode(string roleCode)
        {
            return database.GetList<SYS_ROLEMENUMAP>(new { RoleCode = roleCode });
        }
        public bool InsertRoleMenu(IEnumerable<SYS_ROLEMENUMAP> list)
        {
            var roleCode = list.First().RoleCode;
            try
            {
                database.BeginTransaction();

                database.DeleteList<SYS_ROLEMENUMAP>(new { RoleCode = roleCode });
                database.Insert<SYS_ROLEMENUMAP>(list);

                database.Commit();
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                //TODO：记录异常
                logger.Exception(ex);
                return false;
            }
        }
        

    }
}