/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-05 11:39:58                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISTATIONINFO_SHOWIMGRepository                                      
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

namespace EWF.IRepository
{
    public interface ISTATIONINFO_SHOWIMGRepository : IRepositoryBase<STATIONINFO_SHOWIMG>, IDependency
    {
        IEnumerable<STATIONINFO_SHOWIMG> GetModelById(int ID);
    }
}