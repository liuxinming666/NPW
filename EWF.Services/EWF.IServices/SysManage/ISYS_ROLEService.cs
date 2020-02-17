/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 12:07:55                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IServices                                   
*│　接口名称： ISYS_ROLERepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ISYS_ROLEService: IDependency
    {
        Page<SYS_ROLE> GetUserData(int pageIndex, int pageSize, string rname, string addvcd, int type);
        IEnumerable<SYS_ROLEMENUMAP> GetRoleMenuCode(string roleCode);
        bool InsertRoleMenu(string roleCode, string menuCode);
        SYS_ROLE Get(int ID);
        bool Insert(SYS_ROLE entity);
        bool Update(SYS_ROLE entity);
        bool Delete(int ID);
        bool DeleteList(object whereConditions);
        int IsExists(string rolecode, string addvcd, string type);
        IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename);
    }
}