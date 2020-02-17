/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_ACCESSTIMERepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ISYS_ACCESSTIMEService: IDependency
    {
        /// <summary>
        /// 更新上一次最后登录时间
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="authority"></param>
        /// <returns></returns>
        bool UpdateLastAccessTime(string uid, string authority);
        /// <summary>
        /// 表里插记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        string Insert(SYS_ACCESSTIME entity);
    }
}