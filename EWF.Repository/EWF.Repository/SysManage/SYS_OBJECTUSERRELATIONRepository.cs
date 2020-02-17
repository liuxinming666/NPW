/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_OBJECTUSERRELATIONRepository                                      
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
    public class SYS_OBJECTUSERRELATIONRepository:DefaultRepository<SYS_OBJECTUSERRELATION>, ISYS_OBJECTUSERRELATIONRepository
    {
		public SYS_OBJECTUSERRELATIONRepository(IOptionsSnapshot<DbOption> options):base(options) { }
        /// <summary>
        /// 根据用户ID获取角色列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetRoleListByUID(string userid)
        {
            List<SYS_OBJECTUSERRELATION> list = GetList("where userid='" + userid + "'");
            var strRole = "";
            if (list.Count > 0)
            {
                foreach (SYS_OBJECTUSERRELATION item in list)
                {
                    strRole += item.OBJECTID + ",";
                }
                //拉萨的
                //strRole += userid;
                strRole = strRole.Substring(0, strRole.Length - 1);
            }
            return strRole;
        }


    }
}