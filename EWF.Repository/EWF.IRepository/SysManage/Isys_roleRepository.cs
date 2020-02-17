/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 12:07:55                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISYS_ROLERepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.IRepository.SysManage
{
    public interface ISYS_ROLERepository : IRepositoryBase<SYS_ROLE>, IDependency
    {
        Page<SYS_ROLE> GetUserData(int pageIndex, int pageSize, string rname, string addvcd, int type);
        IEnumerable<SYS_ROLEMENUMAP> GetRoleMenuCode(string roleCode);
        bool InsertRoleMenu(IEnumerable<SYS_ROLEMENUMAP> list);

    }
}