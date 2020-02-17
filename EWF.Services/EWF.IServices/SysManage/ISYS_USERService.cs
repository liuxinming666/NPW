/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:43:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                   
*│　接口名称： ISYS_USERRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ISYS_USERService: IDependency
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="username">名字</param>
        /// <param name="addvcd">区域编码</param>
        /// <param name="type">1行政区划 2流域分区</param>
        /// <returns></returns>
        Page<SYS_USER> GetUserData(int pageIndex, int pageSize, string username, string addvcd, int type);
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="username">用户名字</param>
        /// <param name="addvcd">区域编码</param>
        /// <param name="type">1行政区划 2流域分区</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetUserList(string username, string addvcd, int type);
        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        string Insert(SYS_USER entity);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        string Update(SYS_USER entity);
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="ID">用户id</param>
        /// <returns></returns>
        string Delete(string ID);
        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        /// <param name="ID">用户id</param>
        /// <returns></returns>
        SYS_USER GetUserByID(string ID);
        /// <summary>
        /// 查询用户角色信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        IEnumerable<SYS_OBJECTUSERRELATION> GetRoleByUser(string userid);
        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="category">类型</param>
        /// <returns></returns>
        IEnumerable<SYS_ROLE> GetRoleByCategory(string category);
        /// <summary>
        /// 查询所有角色信息
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <returns></returns>
        IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename);
        /// <summary>
        /// 保存用户角色信息
        /// </summary>
        /// <param name="permissionIds">角色</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        string SaveUserRole(string[] permissionIds, string userid);
        /// <summary>
        /// 判断登录用户名和密码是否正确
        /// </summary>
        /// <param name="userid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        DataTable GetLoginCheck(string userid, string pwd);
        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="user">实体</param>
        /// <returns></returns>
        string UpdateLoginUser(SYS_USER user);
        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <returns></returns>
        DataTable GetUnitData();
        /// <summary>
        /// 获取用户部门信息
        /// </summary>
        /// <param name="unitid">单位id</param>
        /// <returns></returns>
        DataTable GetDepartmenttList(string unitid);
        /// <summary>
        /// 获取行政区划市分区
        /// </summary>
        /// <param name="type">类型：1行政区划 2流域分区</param>
        /// <returns></returns>
        DataTable GetCityData(int type);
        /// <summary>
        /// 获取行政区划市分区
        /// </summary>
        /// <param name="cityid">市</param>
        /// <param name="type">类型：1行政区划 2流域分区</param>
        /// <returns></returns>
        DataTable GetXianData(string cityid, int type);
        /// <summary>
        /// 根据单位id获取单位信息
        /// </summary>
        /// <param name="unitid"></param>
        /// <returns></returns>
        SYS_UNIT GetUnitDataById(string unitid);

        DataTable GetAddvcdData(string addvcd, int type);
    }
}