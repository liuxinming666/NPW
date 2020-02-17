/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 16:58:39                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISYS_DEPARTMENTRepository                                      
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
    public interface ISYS_DEPARTMENTRepository : IRepositoryBase<SYS_DEPARTMENT>, IDependency
    {
        Page<SYS_DEPARTMENT> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName);
        Page<dynamic> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName, string UName, string UnitId);
        /// <summary>
        /// 获取用户部门信息
        /// </summary>
        /// <param name="unitid">单位id</param>
        /// <returns></returns>
        DataTable GetDepartmenttList(string unitid);
    }
}