/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_LOGINSUCCESSRepository                                      
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
    public interface ISYS_LOGINSUCCESSService: IDependency
    {
        /// <summary>
        /// 往表里插记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        string Insert(SYS_LOGINSUCCESS entity);
        /// <summary>
        /// 删除登录成功的记录
        /// </summary>
        /// <param name="userip">ip</param>
        /// <param name="username">用户名</param>
        /// <param name="tm">时间</param>
        /// <returns></returns>
        bool DeleteLoginSuccess(string userip, string username, DateTime tm);
        /// <summary>
        /// 检查登录次数
        /// </summary>
        /// <param name="userip">ip</param>
        /// <param name="username">用户名</param>
        /// <param name="tm">时间</param>
        /// <returns></returns>
        DataTable GetLoginSuccessCount(string userip, string username, DateTime tm);
    }
}