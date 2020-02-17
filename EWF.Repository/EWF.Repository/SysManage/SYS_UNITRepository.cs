/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：lw                                           
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-03 16:58:39                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_UNITRepository                                      
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
    public class SYS_UNITRepository:DefaultRepository<SYS_UNIT>, ISYS_UNITRepository
    {
        public SYS_UNITRepository(IOptionsSnapshot<DbOption> options) : base(options) { }

		/// <summary>
		/// 获取单位分页信息
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="Ucode"></param>
		/// <param name="UnitName"></param>
		/// <returns></returns>
		public Page<SYS_UNIT> GetUnitData(int pageIndex, int pageSize, string Ucode, string UnitName)
        {
            var where = " where 1=1 ";
            var orderby = " SORTCODE ";
            if (!Ucode.IsEmpty())
            {
                where += "and UCode like '" + Ucode + "' ";
            }
            if (!UnitName.IsEmpty())
            {
                where += " and (FULLNAME like '%" + UnitName + "%' or SHORTNAME like '%" + UnitName + "%')";
            }

            //var db = new RepositoryBase(database);
            //var pageuser = db.GetListPaged<SYS_UNIT>(pageIndex, pageSize, where, orderby);
            var pageuser = database.GetListPaged<SYS_UNIT>(pageIndex, pageSize, where, orderby);
            return pageuser;
        }

        /// <summary>
        /// 获取机构单位信息
        /// </summary>
        /// <param name="Ucode"></param>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public DataTable getUnitList(string Ucode, string UnitName)
        {
            string sql = " select * from TBL_SYS_UNIT where 1=1 ";
            if (!Ucode.IsEmpty())
            {
                sql += "and UCode like '" + Ucode + "' ";
            }
            if (!UnitName.IsEmpty())
            {
                sql += " and (FULLNAME like '%" + UnitName + "%' or SHORTNAME like '%" + UnitName + "%')";
            }
            sql += " order by SORTCODE asc ";
            //var db = new RepositoryBase(database);
            //DataTable vtable = db.FindTable(sql);
            DataTable vtable = database.FindTable(sql);
            return vtable;
        }




    }
}