/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISYS_OBJECTUSERRELATIONRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;

namespace EWF.IRepository.SysManage
{
    public interface ISYS_OBJECTUSERRELATIONRepository : IRepositoryBase<SYS_OBJECTUSERRELATION>, IDependency
    {
        /// <summary>
        /// 根据用户ID获取角色列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetRoleListByUID(string userid);
    }
}