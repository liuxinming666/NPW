/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 10:18:16
 * 描  述：MenuService
 * *********************************************/
using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Util;

namespace EWF.Services
{
    public class MenuService : IMenuService
    {
        private IMenuRepository repository;
        public MenuService(IMenuRepository _menuRepository)
        {
            repository = _menuRepository;
        }
        
        /// <summary>
        /// 获取用户顶级菜单
        /// 目前不支持根据用户选择...待扩展
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public string GetUserTopMenu(string userCode)
        {
            DataTable dtMenu = repository.GetUserTopMenu(userCode);
            var json = dtMenu.ToJson();

            return json;
        }
        
        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="parentCode">父菜单编码</param>
        /// <returns></returns>
        public string GetUserMenuByParentCode(string userCode, string parentCode)
        {
            DataTable dtMenu = repository.GetUserMenuByParentCode(userCode, parentCode);


            return CreateMenu(dtMenu, parentCode);
        }

        private string CreateMenu(DataTable dtMenu, string parentCode)
        {

            StringBuilder sb = new StringBuilder();

            DataRow[] rows = dtMenu.Select("ParentCode='" + parentCode + "'");
            sb.Append("[");
            bool isFist = false;
            foreach (DataRow dr in rows)
            {
                if (isFist)
                    sb.Append(",");
                isFist = true;
                string id = dr["MenuCode"].ToString();
                sb.Append("{");
                sb.AppendFormat("\"code\":\"{0}\",", id);
                sb.AppendFormat("\"name\":\"{0}\",", dr["MenuName"]);
                sb.AppendFormat("\"pCode\":\"{0}\",", dr["ParentCode"]);
                sb.AppendFormat("\"iconCls\":\"{0}\",", dr["IconClass"]);
                sb.AppendFormat("\"url\":\"{0}\",", dr["URL"]);
                sb.AppendFormat("\"iconUrl\":\"{0}\",", dr["IconURL"]);
                sb.AppendFormat("\"Description\":\"{0}\"", dr["Description"]);
                sb.Append(",\"submenu\":[");
                sb.Append(GetSubMenu(id, dtMenu));
                sb.Append("]");
                sb.Append("}");
            }
            sb.Append("]");

            return sb.ToString();
        }
        /// <summary>
        /// 递归调用生成无限级别
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSubMenu(string pid, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = dt.Select("ParentCode=" + pid);
            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["MenuCode"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"code\":\"{0}\",", id);
                    sb.AppendFormat("\"name\":\"{0}\",", dr["MenuName"]);
                    sb.AppendFormat("\"pCode\":\"{0}\",", dr["ParentCode"]);
                    sb.AppendFormat("\"iconCls\":\"{0}\",", dr["IconClass"]);
                    sb.AppendFormat("\"url\":\"{0}\",", dr["URL"]);
                    sb.AppendFormat("\"iconUrl\":\"{0}\",", dr["IconURL"]);
                    sb.AppendFormat("\"Description\":\"{0}\"", dr["Description"]);
                    sb.Append(",\"submenu\":[");
                    sb.Append(GetSubMenu(id, dt));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }



        public DataTable GetMenuAll()
        {
            var dt = repository.GetAllMenu();
            return dt;
        }


        public sys_menuEntity Get(int ID)
        {
            var result = repository.Get(ID);
            return result;
        }
        public string Insert(sys_menuEntity entity)
        {
            var result = repository.Insert(entity);
            if (result > 0)
            {
                return "添加菜单成功";
            }
            return "添加菜单失败";
        }

        public string Update(sys_menuEntity entity)
        {
            var result = repository.Update(entity);
            if (result )
            {
                return "修改菜单成功";
            }
            return "修改菜单失败";
        }

        public string Delete(int ID)
        {
            var result = repository.Delete(ID);
            if (result > 0)
            {
                return "删除菜单成功";
            }
            return "删除菜单失败";
        }

        public string DeleteList(object whereConditions)
        {
            var result = repository.DeleteList(whereConditions);
            if (result > 0)
            {
                return "删除菜单成功";
            }
            return "删除菜单失败";
        }
    }
}
