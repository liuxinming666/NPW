/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 10:17:13
 * 描  述：Menu
 * *********************************************/
using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface IMenuService: IDependency
    {
        string GetUserTopMenu(string userCode);
        string GetUserMenuByParentCode(string userCode, string parentCode);
        
        DataTable GetMenuAll();
        string Insert(sys_menuEntity entity);
        string Update(sys_menuEntity entity);
        sys_menuEntity Get(int ID);
        string Delete(int ID);
        string DeleteList(object whereConditions);
    }
}
