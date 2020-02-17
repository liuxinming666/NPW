/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 16:58:39                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISYS_UNITRepository                                      
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
    public interface ISYS_UNITRepository : IRepositoryBase<SYS_UNIT>, IDependency
    {
        Page<SYS_UNIT> GetUnitData(int pageIndex, int pageSize, string Ucode, string UnitName);

        DataTable getUnitList(string Ucode, string UnitName);
    }
}