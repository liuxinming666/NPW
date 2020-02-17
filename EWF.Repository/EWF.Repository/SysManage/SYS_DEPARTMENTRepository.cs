/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-03 16:58:39                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_DEPARTMENTRepository                                      
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
    public class SYS_DEPARTMENTRepository: DefaultRepository<SYS_DEPARTMENT>, ISYS_DEPARTMENTRepository
    {
        public SYS_DEPARTMENTRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{

		}

		public Page<SYS_DEPARTMENT> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName)
        {
            var where = " where 1=1 ";
            var orderby = " SORTCODE ";
            if (!DCode.IsEmpty())
            {
                where += "and DCODE like '" + DCode + "' ";
            }
            if (!DName.IsEmpty())
            {
                where += " and (FULLNAME like '%" + DName + "%' or SHORTNAME like '%" + DName + "%')";
            }

            //var db = new RepositoryBase(database);
            //var pageuser = db.GetListPaged<SYS_DEPARTMENT>(pageIndex, pageSize, where, orderby);
            var pageuser = database.GetListPaged<SYS_DEPARTMENT>(pageIndex, pageSize, where, orderby);
            return pageuser;
        }

        public Page<dynamic> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName, string UName, string UnitId)
        {
            string sqlInnerText = " select d.*,u.FULLNAME as UFULLNAME ,u.SHORTNAME as USHORTNAME,u.ADDRESS as UADDRESS, u.CONTACT as UCONTACT,u.PHONE as UPHONE,u.FAX as UFAX  from TBL_SYS_DEPARTMENT d,TBL_SYS_UNIT U where u.UNITID=d.UNITID and d.unitid='" + UnitId + "' ";
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(DCode))
            {
                sqlInnerText += "and d.DCODE like '" + DCode + "'";
                //sqlParams.Add("dcode", DCode);
            }
            if (!string.IsNullOrEmpty(DName))
            {
                sqlInnerText += " and (d.FULLNAME like '%" + DName + "%' or d.SHORTNAME like '%" + DName + "%')";
                sqlParams.Add("dname", DName);
            }
            if (!string.IsNullOrEmpty(UName))
            {
                sqlInnerText += " and u.FULLNAME like '%" + UName + "%'";
                //sqlParams.Add("uname", UName);
            }
            var tableName = "(" + sqlInnerText + ") aa  ";
            var flied = "aa.*";
            var where = "1=1";
            var orderby = "SORTCODE asc";
            //var db = new RepositoryBase(database);
            //var page = db.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby);
            var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby);
            return page;
        }
        public DataTable GetDepartmenttList(string unitid)
        {
            string sql = " select * from TBL_SYS_department where 1=1 and ISENABLED=1 and UNITID='" + unitid + "'";
            //if (!unitid.IsEmpty())
            //{
            //    sql += "and UNITID='" + unitid + "' ";
            //}
            sql += " order by SORTCODE asc ";
            DataTable vtable = database.FindTable(sql);
            return vtable;
        }



    }
}