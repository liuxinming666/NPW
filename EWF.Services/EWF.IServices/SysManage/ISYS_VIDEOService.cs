/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_USERRepository                                      
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
    public interface ISYS_VIDEOService: IDependency
    {
        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="username">名字</param>
        /// <returns></returns>
        Page<SYS_VIDEO> GetVideoData(int pageIndex, int pageSize, string name, int type, string addvcd);
        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Insert(SYS_VIDEO entity);
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Update(SYS_VIDEO entity);
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="ID">用户id</param>
        /// <returns></returns>
        bool Delete(int ID);
        /// <summary>
        /// 根据id获取实体信息
        /// </summary>
        /// <param name="ID">用户id</param>
        /// <returns></returns>        
        SYS_VIDEO GetVideoByID(int ID);
        /// <summary>
        /// 根据站码获取实体列表
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        IEnumerable<SYS_VIDEO> GetVideoBySTCD(string STCD);
    }
}