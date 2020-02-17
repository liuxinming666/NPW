/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-05 12:20:06                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IServices                                   
*│　接口名称： ISTATIONINFO_SURVEYRepository                                      
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
    public interface ISTATIONINFO_SURVEYService: IDependency
    {
        Page<STATIONINFO_SURVEY> GetPageData(int pageIndex, int pageSize, string name);
        int Count(string STCD);
        STATIONINFO_SURVEY Get(Int32 ID);
        bool Insert(STATIONINFO_SURVEY entity);
        bool Update(STATIONINFO_SURVEY entity);
        bool Delete(Int32 ID);
        bool DeleteList(object whereConditions);
        /// <summary>
        /// 根据站码获取实体列表
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        IEnumerable<STATIONINFO_SURVEY> GetModelBySTCD(string STCD);
    }
}