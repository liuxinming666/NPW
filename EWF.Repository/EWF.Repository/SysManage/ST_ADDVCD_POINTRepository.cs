using Dapper;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.IRepository;
using EWF.Util;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Repository
{
    public  class ST_ADDVCD_POINTRepository : DefaultRepository<ST_ADDVCD_POINT>, IST_ADDVCD_POINTRepository
    {
        public ST_ADDVCD_POINTRepository(IOptionsSnapshot<DbOption> options):base(options)
        {
        }

        /// <summary>
        /// 获取行政区划分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ADDVCD"></param>
        /// <param name="ADDVNM"></param>
        /// <returns></returns>
        public Page<ST_ADDVCD_POINT> GetAddvcdPointData(int pageIndex, int pageSize,string Addvcd, string Stnm)
        {
            var where = " where ADDVCD ='" + Addvcd + "' ";
            var orderby = " ADDVCD ";
            if (!Stnm.IsEmpty())
            {
                where += " and STNM like '%" + Stnm + "%' ";
            }
            var db = new RepositoryBase(database);
            //var pageuser = db.GetListPaged<ST_ADDVCD_D>(pageIndex, pageSize, where, orderby);
            var pageuser = GetListPaged(pageIndex, pageSize, where, orderby);
            return pageuser;
        }

        public Page<dynamic> GetAddPointData(int pageIndex, int pageSize, string Addvcd,  string Stnm)
        {
            string sqlInnerText = " select stcd,stnm,addvcd,type from  ST_STBPRP_V  where addvcd='"+ Addvcd + "' ";
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(Stnm))
            {
                sqlInnerText += " and stnm like '%" + Stnm + "%'";
                //sqlParams.Add("uname", UName);
            }
            var tableName = "(" + sqlInnerText + ") aa  ";
            var flied = "aa.*";
            var where = "1=1";
            var orderby = " stcd asc";
            var db = new RepositoryBase(database);
            var page = db.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby);
            return page;
        }


        public Page<dynamic> GetStcdData(int pageIndex, int pageSize, string Stcd, string Stnm)
        {
            string sqlInnerText = " select STCD,STNM, STTP from  RWDB.[dbo].[ST_STBPRP_B] where 1=1 ";
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(Stcd))
            {
                sqlInnerText += " and STCD like '" + Stcd + "%'";
                //sqlParams.Add("uname", UName);
            }
            if (!string.IsNullOrEmpty(Stnm))
            {
                sqlInnerText += " and STNM like '%" + Stnm + "%'";
                //sqlParams.Add("uname", UName);
            }
            var tableName = "(" + sqlInnerText + ") aa  ";
            var flied = "aa.*";
            var where = "1=1";
            var orderby = " stcd asc";
            var db = new RepositoryBase(database);
            var page = db.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby);
            return page;
        }

        public int DeletePoint(string Addvcd, string Stcd)
        {
            string sql = "delete from ST_ADDVCD_POINT where addvcd='"+Addvcd+"' and stcd = '"+ Stcd + "'";
            var db = new RepositoryBase(database);
            int value = db.ExecuteBySql(sql);
            return value;
        } 
    }
}
