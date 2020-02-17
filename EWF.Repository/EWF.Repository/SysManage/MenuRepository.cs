/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 10:30:32
 * 描  述：MenuRepository
 * *********************************************/
using EWF.Data.Repository;
using EWF.Entity;
using EWF.IRepository;
using EWF.Util;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Repository
{
    public class MenuRepository : RepositoryBase<sys_menuEntity>, IMenuRepository
    {
        public MenuRepository(IOptionsSnapshot<DbOption> options)
        {
            dbOption = options.Get("Default_Option");
            if (dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
        }

        public DataTable GetAllMenu()
        {
            string sql = "select A.*,B.MenuName as ParentName from sys_menu A left join sys_menu B on B.MenuCode = A.ParentCode order by A.MenuSeq,A.MenuCode";
            DataTable dtMenu = database.FindTable(sql);
            return dtMenu;
        }

        /// <summary>
        /// 获取用户顶级菜单
        /// 目前不支持根据用户选择...待扩展
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public DataTable GetUserTopMenu(string userCode)
        {
            string sql = string.Empty;
            if (!userCode.IsEmpty())
            {
                sql = string.Format("SELECT distinct a.[MenuCode],a.[ParentCode],a.[MenuName],a.[URL],a.[IconClass],a.[IconURL],a.[MenuSeq],a.[Description],a.[IsVisible],a.[IsEnable],a.[CreatePerson],a.[CreateDate],a.[UpdatePerson],a.[UpdateDate] FROM sys_menu a,tbl_sys_role b,tbl_sys_rolemenumap c where a.menucode=c.menucode and b.rolecode=c.rolecode and b.id in ({0}) and len(a.MenuCode)=2 and IsEnable=1 and IsVisible=1 order by MenuSeq", userCode);
            }
            else
                sql = string.Format("SELECT distinct a.[MenuCode],a.[ParentCode],a.[MenuName],a.[URL],a.[IconClass],a.[IconURL],a.[MenuSeq],a.[Description],a.[IsVisible],a.[IsEnable],a.[CreatePerson],a.[CreateDate],a.[UpdatePerson],a.[UpdateDate] FROM sys_menu a,tbl_sys_role b,tbl_sys_rolemenumap c where a.menucode=c.menucode and b.rolecode=c.rolecode and b.id in ('{0}') and len(a.MenuCode)=2 and IsEnable=1 and IsVisible=1 order by MenuSeq", userCode);
            //sql = "SELECT [MenuCode],[ParentCode],[MenuName],[URL],[IconClass],[IconURL],[MenuSeq],[Description],[IsVisible],[IsEnable],[CreatePerson],[CreateDate],[UpdatePerson],[UpdateDate] FROM sys_menu where len(MenuCode)=2 and IsEnable=1 and IsVisible=1 order by MenuSeq";
            DataTable dtMenu = database.FindTable(sql);
            return dtMenu;
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="parentCode">父菜单编码</param>
        /// <returns></returns>
        public DataTable GetUserMenuByParentCode(string userCode, string parentCode)
        {            
            string sql = string.Empty;
            //sql = string.Format("SELECT a.[MenuCode],[ParentCode],[MenuName],[URL],[IconClass],[IconURL],[MenuSeq],[Description],[IsVisible],[IsEnable],[CreatePerson],[CreateDate],[UpdatePerson],[UpdateDate] FROM sys_menu a,sys_roleMenuMap b,sys_userRoleMap c where b.MenuCode=a.MenuCode and b.RoleCode=c.RoleCode and c.UserCode='{0}'  and left([ParentCode],2)={1} and IsEnable=1 and IsVisible=1 order by MenuSeq",userCode, parentCode);

            //sql = string.Format("SELECT [MenuCode],[ParentCode],[MenuName],[URL],[IconClass],[IconURL],[MenuSeq],[Description],[IsVisible],[IsEnable],[CreatePerson],[CreateDate],[UpdatePerson],[UpdateDate] FROM sys_menu where left([ParentCode],2)='{0}' and IsEnable=1 and IsVisible=1 order by MenuSeq",  parentCode);
            if (!userCode.IsEmpty())
            {
                sql = string.Format("SELECT distinct a.[MenuCode],a.[ParentCode],a.[MenuName],a.[URL],a.[IconClass],a.[IconURL],a.[MenuSeq],a.[Description],a.[IsVisible],a.[IsEnable],a.[CreatePerson],a.[CreateDate],a.[UpdatePerson],a.[UpdateDate] FROM sys_menu a,tbl_sys_role b,tbl_sys_rolemenumap c where a.menucode=c.menucode and b.rolecode=c.rolecode and b.id in ({0}) and left([ParentCode],2)='{1}' and IsEnable=1 and IsVisible=1 order by MenuSeq", userCode, parentCode);
            }
            else
                sql = string.Format("SELECT distinct a.[MenuCode],a.[ParentCode],a.[MenuName],a.[URL],a.[IconClass],a.[IconURL],a.[MenuSeq],a.[Description],a.[IsVisible],a.[IsEnable],a.[CreatePerson],a.[CreateDate],a.[UpdatePerson],a.[UpdateDate] FROM sys_menu a,tbl_sys_role b,tbl_sys_rolemenumap c where a.menucode=c.menucode and b.rolecode=c.rolecode and b.id in ('{0}') and left([ParentCode],2)='{1}' and IsEnable=1 and IsVisible=1 order by MenuSeq", userCode, parentCode);
            DataTable dtMenu = database.FindTable(sql);
            return dtMenu;
        }
        



    }
}
