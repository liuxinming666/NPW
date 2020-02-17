/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_LOGINERROERepository                                      
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
    public interface ISYS_LOGINERROEService: IDependency
    {
        /// <summary>
        /// 查询登录错误次数
        /// </summary>
        /// <param name="userip">ip</param>
        /// <param name="username">用户名</param>
        /// <param name="tm">时间</param>
        /// <returns></returns>
        DataTable GetLoginErrorCount(string userip, string username, DateTime tm);
        /// <summary>
        /// 往表里插记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        string Insert(SYS_LOGINERROE entity);
        /// <summary>
        /// 查询一年登录错误次数
        /// </summary>
        /// <param name="userip">ip</param>
        /// <param name="username">用户名</param>
        /// <param name="tm">时间</param>
        /// <param name="ishassql"></param>
        /// <returns></returns>
        DataTable GetLoginByHasSQLCount(string userip, string username, DateTime tm, string ishassql);
        /// <summary>
        /// 删除登录失败的记录
        /// </summary>
        /// <param name="userip">ip</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        bool DeleteLoginError(string userip, string username);
    }
}