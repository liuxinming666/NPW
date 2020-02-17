/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：IMenuRepository
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;

namespace EWF.IRepository
{
    public interface IMenuRepository : IRepositoryBase<sys_menuEntity>,IDependency
    {
        DataTable GetAllMenu();
        DataTable GetUserTopMenu(string userCode);
        DataTable GetUserMenuByParentCode(string userCode, string parentCode);
    }
}
