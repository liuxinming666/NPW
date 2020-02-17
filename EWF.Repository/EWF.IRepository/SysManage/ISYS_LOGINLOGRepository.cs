/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.IRepository                                   
*│　接口名称： ISYS_LOGINLOGRepository                                      
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
    public interface ISYS_LOGINLOGRepository : IRepositoryBase<SYS_LOGINLOG>, IDependency
    {
        /// <summary>
        /// 获取登录日志信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="logtype">日志类型</param>
        /// <returns></returns>
        Page<SYS_LOGINLOG> GetLoginLogData(int pageIndex, int pageSize, string logtype);
    }
}