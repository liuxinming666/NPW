/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_LOGINLOGRepository                                      
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

namespace EWF.Repository
{
    public class SYS_LOGINLOGRepository:DefaultRepository<SYS_LOGINLOG>, ISYS_LOGINLOGRepository
    {
        public SYS_LOGINLOGRepository(IOptionsSnapshot<DbOption> options): base(options)
		{
		}
		public Page<SYS_LOGINLOG> GetLoginLogData(int pageIndex, int pageSize, string logtype)
        {
            var where = "";
            var orderby = "SLDATETIME desc";
            if (!logtype.IsEmpty())
            {
                where = "where SLTYPE like '%" + logtype + "%'";
            }
            var pagedata = GetListPaged(pageIndex, pageSize, where, orderby);
            return pagedata;
        }



    }
}