/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISYS_VIDEO                                      
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
    public interface ISYS_VIDEOMANAGERepository : IRepositoryBase<SYS_VIDEOMANAGE>, IDependency
    {
        /// <summary>
        /// 查询摄像头信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="username">名字</param>
        /// <returns></returns>
        Page<SYS_VIDEOMANAGE> GetCameraData(int pageIndex, int pageSize, string name, int type, string addvcd);
        IEnumerable<SYS_VIDEOMANAGE> GetCameraNameList(string SNAME);
        IEnumerable<SYS_VIDEOMANAGE> GetCameraList(string STCD, string SNAME, string SIP, string SPORT);
        IEnumerable<SYS_VIDEOMANAGE> GetCameraListBySTCD(string STCD);
        SYS_VIDEOMANAGE GetCameraByID(int STCD);
    }
}