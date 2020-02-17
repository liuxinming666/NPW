/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_USERRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository.SysManage;
using EWF.Util.Options;
using EWF.Util;
using EWF.Entity;
using EWF.Util.Page;
using System.Data;

namespace EWF.Repository
{
    public class SYS_USERRepository:DefaultRepository<SYS_USER>, ISYS_USERRepository
    {
        public SYS_USERRepository(IOptionsSnapshot<DbOption> options) : base(options) { }
		public Page<SYS_USER> GetUserData(int pageIndex, int pageSize, string username, string addvcd, int type)
        {
            var condition = "";
            var orderby = "UACCOUNT";
            condition= " where addvcd='" + addvcd + "' and type=" + type;
            if (!username.IsEmpty())
            {
                condition += " and (REALNAME like '%" + username + "%' or UACCOUNT like '%" + username + "%') ";
            }
            var pageuser = GetListPaged(pageIndex, pageSize, condition, orderby);
            return pageuser;
        }
        public IEnumerable<dynamic> GetUserList(string username, string addvcd, int type)
        {
            string sql = string.Empty;
            if (!addvcd.IsEmpty())
            {
                if (type == 1)
                {
                    if (addvcd.Substring(5, 2) == "00")
                    {
                        sql = "SELECT A.*,B.FULLNAME AS UNITNAME,C.FULLNAME AS DEPTNAME FROM TBL_SYS_USER A LEFT JOIN TBL_SYS_UNIT B ON A.UNITID=B.UNITID LEFT JOIN TBL_SYS_DEPARTMENT C ON A.DEPARTMENTID=C.DEPARTMENTID where substring(a.addvcd,1,4)='" + addvcd.Substring(1, 4) + "' and a.type=" + type;
                    }
                    else
                        sql = "SELECT A.*,B.FULLNAME AS UNITNAME,C.FULLNAME AS DEPTNAME FROM TBL_SYS_USER A LEFT JOIN TBL_SYS_UNIT B ON A.UNITID=B.UNITID LEFT JOIN TBL_SYS_DEPARTMENT C ON A.DEPARTMENTID=C.DEPARTMENTID where a.addvcd='" + addvcd + "' and a.type=" + type;

                }
                else
                {
                    sql = "SELECT A.*,B.FULLNAME AS UNITNAME,C.FULLNAME AS DEPTNAME FROM TBL_SYS_USER A LEFT JOIN TBL_SYS_UNIT B ON A.UNITID=B.UNITID LEFT JOIN TBL_SYS_DEPARTMENT C ON A.DEPARTMENTID=C.DEPARTMENTID where a.addvcd='" + addvcd + "' and a.type=" + type;
                    //sql = "SELECT A.*,B.FULLNAME AS UNITNAME,C.FULLNAME AS DEPTNAME FROM TBL_SYS_USER A LEFT JOIN TBL_SYS_UNIT B ON A.UNITID=B.UNITID LEFT JOIN TBL_SYS_DEPARTMENT C ON A.DEPARTMENTID=C.DEPARTMENTID where 1=1 ";                
                }
            }
            else
                sql = "SELECT A.*,B.FULLNAME AS UNITNAME,C.FULLNAME AS DEPTNAME FROM TBL_SYS_USER A LEFT JOIN TBL_SYS_UNIT B ON A.UNITID=B.UNITID LEFT JOIN TBL_SYS_DEPARTMENT C ON A.DEPARTMENTID=C.DEPARTMENTID where 1=1 ";                
            if (!username.IsEmpty())
            {
                sql += " and (REALNAME like '%" + username + "%' or UACCOUNT like '%" + username + "%') ";
            }
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sql);
            }
        }


        public IEnumerable<SYS_OBJECTUSERRELATION> GetRoleByUser(string userid)
        {
            string sql = string.Empty;
            sql = string.Format("SELECT * FROM TBL_SYS_OBJECTUSERRELATION where USERID='{0}'", userid);           
            using (var db = database.Connection)
            {
                return db.Query<SYS_OBJECTUSERRELATION>(sql);
            }
        }
        public IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename)
        {
            //如果rolename不为空，说明不是管理员的角色
            string sql = string.Empty;

            sql = "SELECT * FROM TBL_SYS_ROLE where addvcd='" + addvcd + "' and type=" + type;
            if (!roleid.IsEmpty())
                sql += " and id in(" + roleid + ") and rolename = '超级管理员'";
            if (!rolename.IsEmpty())
                sql += " and rolename != '超级管理员'";
            using (var db = database.Connection)
            {
                return db.Query<SYS_ROLE>(sql);
            }
        }
        public IEnumerable<SYS_ROLE> GetRoleByCategory(string category)
        {
            string sql = string.Empty;

            sql = string.Format("SELECT * FROM TBL_SYS_ROLES where CATEGORY='{0}'  and ISENABLED=1 ", category);
            using (var db = database.Connection)
            {
                return db.Query<SYS_ROLE>(sql);
            }
        }

        public string SaveUserRole(string[] permissionIds, string userid)
        {
            var result = "";
            int i = 1;
            database.BeginTransaction();
           
            database.DeleteList<SYS_OBJECTUSERRELATION>(new { USERID = userid });
            foreach (var itemId in permissionIds)
            {
                SYS_OBJECTUSERRELATION userRoleEntity = new SYS_OBJECTUSERRELATION();
                userRoleEntity.OBJECTUSERRELATIONID = Guid.NewGuid().ToString("N").ToUpper();
                userRoleEntity.USERID = userid;
                userRoleEntity.OBJECTID = itemId;
                userRoleEntity.SORTCODE = i;
                i++;
                userRoleEntity.CREATEDATE = DateTime.Now;
                userRoleEntity.CREATEUSERID = userid;
                userRoleEntity.CREATEUSERNAME = userid;
                result = database.Insert<string, SYS_OBJECTUSERRELATION>(userRoleEntity);
            }
            database.Commit();
            return result;
        }
        //判断登录的用户是否存在
        public DataTable GetLoginCheck(string userid, string pwd)
        {
            string sql = string.Empty;
            sql = string.Format("SELECT USERID,A.UACCOUNT,A.UPWD,REALNAME,A.UNITID,B.FULLNAME AS UNITNAME,A.DEPARTMENTID,C.FULLNAME AS DEPTNAME,FIRSTVISIT,LASTVISIT,A.ISENABLED,A.ADDVCD,A.TYPE FROM TBL_SYS_USER A LEFT JOIN TBL_SYS_UNIT B ON B.UNITID=A.UNITID LEFT JOIN TBL_SYS_DEPARTMENT C ON C.DEPARTMENTID=A.DEPARTMENTID WHERE A.UACCOUNT='{0}' AND A.UPWD='{1}'", userid, pwd);
            return database.FindTable(sql);
        }
        //获取行政区划市,先取郑州市的
        public DataTable GetCityData(int type)
        {
            string sql = "";
            if (type == 1)
            {
                sql = "SELECT ADDVCD, ADDVNM, COMMENTS, MODITIME, TYPE FROM ST_ADDVCD_D WHERE ADDVCD LIKE '4101%' AND right(addvcd,2)='00' AND TYPE=1";
            }
            else
                sql = "SELECT ADDVCD, ADDVNM, COMMENTS, MODITIME, TYPE FROM ST_ADDVCD_D WHERE TYPE=2";
            return database.FindTable(sql);
        }
        //获取行政区划县,先取郑州市的
        public DataTable GetXianData(string cityid, int type)
        {
            string sql = "";
            if (type == 1)
            {
                sql = "SELECT ADDVCD, ADDVNM, COMMENTS, MODITIME, TYPE FROM ST_ADDVCD_D WHERE ADDVCD LIKE '4101%' AND right(addvcd,2)!='00' AND TYPE=1";
            }
            else
            {
                var city = "";
                if (!cityid.IsEmpty())
                {
                    city = cityid.Substring(0, 4);
                }
                sql = "SELECT ADDVCD, ADDVNM, COMMENTS, MODITIME, TYPE FROM ST_ADDVCD_D WHERE ADDVCD LIKE '" + city + "%' AND right(addvcd,2)!='00' AND TYPE=2";
            }
            return database.FindTable(sql);
        }
        public DataTable GetAddvcdData(string addvcd, int type)
        {
            string sql = "";
            sql = "SELECT ADDVCD, ADDVNM, COMMENTS, MODITIME, TYPE FROM ST_ADDVCD_D WHERE ADDVCD ='" + addvcd + "' and type=" + type;
            return database.FindTable(sql);
        }


    }
}