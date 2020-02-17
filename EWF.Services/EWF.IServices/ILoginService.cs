using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ILoginService: IDependency
    {
        /// <summary>
        /// 根据IP获取黑名单
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        dynamic GetBackUser(string ip);
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="userpwd">密码</param>
        /// <param name="userip">ip地址</param>
        /// <param name="host">主机地址</param>
        /// <param name="path">模块</param>
        /// <returns></returns>
        string LoginCheck(string username, string userpwd, string userip, string host, string path, out DataTable dtCheckUser, out string roleid);
    }
}
