using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using EWF.Util;
using EWF.Entity;
using EWF.Util.Page;
using System.Data;

namespace EWF.Repository
{
    public class SysConfigRepository : DefaultRepository<SYS_Config>, ISysConfigRepository
    {
        public SysConfigRepository(IOptionsSnapshot<DbOption> options) : base(options) { }

		public IEnumerable<dynamic> GetSysConfigData(int sysId)
        {
            //分组排序sql语句
            string strSqlInnerText = " SELECT SYSID ,SYSNAME,SYSLOGO ,SYSBGPIC,SYSCONTENT,SYSCOL  FROM TBL_SYS_SYSCONFIG where ";
           //参数列表
            var sqlParams = new DynamicParameters();
            if (sysId>0)
            {
                strSqlInnerText += "  SYSID=@SYSID ";
                sqlParams.Add("SYSID", sysId);
            }
           
            //获取每个分组中序号为1的数据，即最新一条数据
            string strSqlOuterText = strSqlInnerText;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }
        /// <summary>
        /// 根据区域代码获取系统配置
        /// </summary>
        /// <param name="addvcd"></param>
        /// <returns></returns>
        public IEnumerable<SYS_Config> GetSysConfigByAddvcd(string addvcd)
        {
            string strSqlInnerText = " SELECT SYSID ,SYSNAME,SYSLOGO ,SYSBGPIC,SYSCONTENT,SYSCOL,ADDVCD,STCD,LGLT,VIDEONAME,VIDEOSTCD  FROM TBL_SYS_SYSCONFIG where ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(addvcd))
            {
                strSqlInnerText += "  ADDVCD=@ADDVCD ";
                sqlParams.Add("ADDVCD", addvcd);
            }
            using (var db = database.Connection)
            {
                return db.Query<SYS_Config>(strSqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 根据系统配置的测站编码获取测站名称
        /// </summary>
        /// <param name="addvcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStnmBySysconfig(string addvcd)
        {
            string sql = $"select * from {Default_Schema}ST_STBPRP_V where 1=1";
            string sqlConfigStcd = $"select replace(c.stcd,',',''',''') as stcds from {Default_Schema}TBL_SYS_SYSCONFIG C where 1=1 and addvcd='" + addvcd + "'";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(addvcd))
            {
                sql += " and ADDVCD=@ADDVCD ";
                sqlParams.Add("ADDVCD", addvcd);
            }
            DataTable dtStcd = new DataTable();
            dtStcd = database.FindTable(sqlConfigStcd);
            string stcds = "";
            stcds = "'" + dtStcd.Rows[0]["stcds"].ToString() + "'";
            sql += " and stcd in(" + stcds + ") order by stcd";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sql, sqlParams);
            }
        }

        public DataTable GetStationListByStnm(string stnm)
        {
            string sql = $"SELECT * FROM {Default_Schema}ST_STBPRP_V WHERE stnm = '" + stnm.Trim() + "'";
            return database.FindTable(sql);
        }

    }
}
