/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_UNITRepository                                      
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
    public interface ISYS_UNITService: IDependency
    {
        /// <summary>
        /// 获取单位分页信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Ucode"></param>
        /// <param name="unitname"></param>
        /// <returns></returns>
        Page<SYS_UNIT> GetUnitData(int pageIndex, int pageSize,string Ucode, string unitname);

        DataTable getUnitList(string UCode, string UnitName);

        string Insert(SYS_UNIT entity);

        string Update(SYS_UNIT entity);

        string Delete(string ID);

        SYS_UNIT GetUnitByID(string ID);
        

    }
}