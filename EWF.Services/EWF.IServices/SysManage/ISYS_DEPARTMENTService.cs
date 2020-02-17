/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_DEPARTMENTRepository                                      
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
    public interface ISYS_DEPARTMENTService: IDependency
    {
        Page<SYS_DEPARTMENT> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName);
        /// <summary>
        /// 获取部门分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="DCode"></param>
        /// <param name="DName"></param>
        /// <param name="UName"></param>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        Page<dynamic> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName, string UName, string UnitId);

        string Insert(SYS_DEPARTMENT entity);

        string Update(SYS_DEPARTMENT entity);

        string Delete(string ID);

        SYS_DEPARTMENT GetUnitByID(string ID);
    }
}