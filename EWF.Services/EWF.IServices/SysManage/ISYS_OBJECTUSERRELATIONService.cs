/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_OBJECTUSERRELATIONRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ISYS_OBJECTUSERRELATIONService: IDependency
    {
        /// <summary>
        /// 根据用户ID获取角色列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetRoleListByUID(string userid);
    }
}