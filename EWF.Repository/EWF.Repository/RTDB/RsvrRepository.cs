/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：lw
 * 日  期：2019/5/24 16:36:04
 * 描  述：RsvrRepository
 * *********************************************/
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
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
    public class RsvrRepository : DefaultRepository,IRsvrRepository
	{
		private readonly string ST_STBPRP_V;
		private readonly string ST_RSVR_R;
		private readonly string ST_RSVRAV_R;

		public RsvrRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			ST_RSVR_R = $"{RTDB_Schema}ST_RSVR_R";
			ST_RSVRAV_R = $"{RTDB_Schema}ST_RSVRAV_R";
		}
		
        /// <summary>
        /// 获取指定时段内最新水库水情数据
        /// add by lw
        /// Date:2019-05-24 10:00
        /// </summary>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区编码</param>
        /// <param name="type">结束时间，为空则结束时间为表中最后时间</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetLatestRsvr(string startDate, string endDate, string addvcd, string type)
        {
            #region 之前的
            ////分组排序sql语句
            //string strSqlInnerText = "select t.*, ROW_NUMBER()over(partition by t.STCD order by t.TM desc) as ROWNUM from " +
            //    "(select s.*,r.avinq,r.avotq,r.avrz,r.avw, rtrim(b.stnm) as stnm,rtrim(b.rvnm) as rvnm ,rtrim(b.hnnm) as hnnm,b.bsnm,b.lgtd,b.lttd from st_rsvr_r s inner join  st_stbprp_b b " +
            //    "on s.stcd = b.stcd  left join  st_rsvrav_r r on s.stcd = r.stcd and s.tm = r.idtm ) t where 1=1 ";
            ////参数列表
            //var sqlParams = new DynamicParameters();
            //if (!string.IsNullOrEmpty(startDate))
            //{
            //    strSqlInnerText += " and t.TM>@StartDate";
            //    sqlParams.Add("StartDate", startDate);
            //}
            //if (!string.IsNullOrEmpty(endDate))
            //{
            //    strSqlInnerText += " and t.TM<=@EndDate";
            //    sqlParams.Add("EndDate", endDate);
            //}
            ////获取每个分组中序号为1的数据，即最新一条数据
            //string strSqlOuterText = "select STCD,TM,RWPTN,RZ,INQ,W,OTQ,INQDR,AVINQ,AVOTQ,AVW,STNM,RVNM,HNNM,BSNM,LGTD,LTTD  from (" + strSqlInnerText + ") aa where ROWNUM=1";
            #endregion
            //参数列表
            var sqlParams = new DynamicParameters();
            sqlParams.Add("StartDate", startDate);
            sqlParams.Add("EndDate", endDate);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);

			////分组排序sql语句
			//string strSqlInnerText = "select t.*, ROW_NUMBER()over(partition by t.STCD order by t.TM desc) as ROWNUM from " +
			//    "(select s.*,r.avinq,r.avotq,r.avrz,r.avw, rtrim(b.stnm) as stnm,rtrim(b.rvnm) as rvnm ,rtrim(b.hnnm) as hnnm,b.bsnm,b.lgtd,b.lttd from st_rsvr_r s inner join  (select * from [EW_NPW].[dbo].[ST_STBPRP_V]  where ADDVCD=@ADDVCD and TYPE=@TYPE) b " +
			//    "on s.stcd = b.stcd  left join  st_rsvrav_r r on s.stcd = r.stcd and s.tm = r.idtm ) t where 1=1 ";

			var strSqlInnerText = "select s.*,r.avinq,r.avotq,r.avrz,r.avw, rtrim(b.stnm) as stnm,rtrim(b.rvnm) as rvnm,rtrim(b.hnnm) as hnnm,b.bsnm,b.lgtd,b.lttd," +
								"ROW_NUMBER()over(partition by s.STCD order by s.TM desc) as ROWNUM,c.FSLTDZ,case when s.rz-c.FSLTDZ > 0  then '水位超汛限'+cast(s.rz-c.FSLTDZ as varchar) end as remark " +
								"from " + ST_RSVR_R + " s inner join  " + ST_STBPRP_V + " b on b.stcd=s.stcd left join  " + ST_RSVRAV_R + " r on s.stcd = r.stcd and s.tm = r.idtm " +
								"left join " + RTDB_Schema + "ST_RSVRFSR_B c on s.stcd= c.stcd where 1=1 ";
			if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and s.TM>@StartDate";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and s.TM<=@EndDate";
            }

            if (!string.IsNullOrEmpty(addvcd))
            {
                strSqlInnerText += " and b.ADDVCD=@ADDVCD";
            }
            if (!string.IsNullOrEmpty(type))
            {
                strSqlInnerText += " and b.TYPE=@TYPE";
            }
            //获取每个分组中序号为1的数据，即最新一条数据
            //string strSqlOuterText = "select STCD,TM,RWPTN,RZ,INQ,W,OTQ,INQDR,AVINQ,AVOTQ,AVW,STNM,RVNM,HNNM,BSNM,LGTD,LTTD  from (" + strSqlInnerText + ") aa where ROWNUM=1";
            string strSqlOuterText = "select STCD,TM,RWPTN,RZ,INQ,W,OTQ,INQDR,AVINQ,AVOTQ,AVW,STNM,RVNM,HNNM,BSNM,LGTD,LTTD,REMARK  from (" + strSqlInnerText + ") aa where ROWNUM=1";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }

        public Page<dynamic> GetReadData(int pageIndex, int pageSize, string STNM)
        {
			var tableName = ST_RSVR_R + " tba," + RTDB_Schema + "ST_STBPRP_B tbb";
            var flied = "tbb.STNM,tbb.RVNM,tbb.HNNM,tbb.BSNM, tba.* ";
            var where = "tba.stcd=tbb.stcd and TM>'2016-10-24 14:00:00.000'";
            var orderby = "TM DESC";
			if (!STNM.IsEmpty())
			{
				where += string.Format(" AND tbb.STNM like '%{0}%'", STNM);
			}

			var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby);
            return page;
        }

        /// <summary>
        /// 查询水库水情信息
        /// </summary>
        /// <param name="stcds">站码列表，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区编码</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public Page<dynamic> GetRsvrData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type)
        {
            //获取水位、流量、
            //string sqlInnerText = "select  A.STNM,A.RVNM,A.HNNM,A.BSNM,B.* "
            //                     + "  from ST_STBPRP_B A, ST_RSVR_R B where A.STCD = B.STCD ";
            string sqlInnerText = "select  A.STNM,A.RVNM,A.HNNM,A.BSNM,B.* "
                                + "  from "+ ST_STBPRP_V + " A, " + ST_RSVR_R + " B where A.STCD = B.STCD ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(addvcd))
            {
                sqlInnerText += " and A.ADDVCD =@ADDVCD";
                sqlParams.Add("ADDVCD", addvcd);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sqlInnerText += " and A.TYPE=@TYPE";
                sqlParams.Add("TYPE",type);
            }
            if (!string.IsNullOrEmpty(stcds))
            {
                //sqlInnerText += " and A.STCD in (@STCDS)";
                sqlInnerText += " and A.STCD =@STCDS";
                sqlParams.Add("STCDS", stcds);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and TM>=@StartDate";
                sqlParams.Add("StartDate", startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            var tableName = "(" + sqlInnerText + ") aa left join "+ST_RSVRAV_R+" bb on aa.STCD=bb.STCD and aa.TM=bb.IDTM and STTDRCD=1 ";
            var flied = "aa.*,BB.AVRZ,BB.AVINQ,bb.AVOTQ,bb.AVW";
            var where = "1=1";
            var orderby = "aa.STCD ASC,TM DESC";
            var db = new RepositoryBase(database);
            //var page = database.GetListPaged<>
            //var page = db.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby, sqlParams);
            var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby, sqlParams);
            return page;
        }

        /// <summary>
        /// 查询水库水情均值
        /// add by  lw
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRsvrav_avg(int sttdrcd, string stcd, string startDate, string endDate)
        {
            //分组排序sql语句
            string strSqlInnerText = $"select a.*,b.STCD from {ST_RSVRAV_R} a,{ST_STBPRP_V} b where a.STCD=b.STCD ";
            //参数列表;
            var sqlParams = new DynamicParameters();
            strSqlInnerText += " and a.STTDRCD=@STTDRCD ";
            sqlParams.Add("STTDRCD", sttdrcd);
            if (!string.IsNullOrEmpty(stcd))
            {
                strSqlInnerText += " and a.STCD in (@STCDS)";
                sqlParams.Add("STCDS", stcd);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and a.IDTM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and a.IDTM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            //获取每个分组中序号为1的数据，即最新一条数据
            strSqlInnerText += " order by a.IDTM desc";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlInnerText, sqlParams);
            }
        }

        /// <summary>
        /// 查询水库水情过程线信息
        /// add by lw
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRsvr_Line(string stcd, string startDate, string endDate)
        {
			//分组排序sql语句
			string strSqlInnerText = "select a.STCD,a.TM, a.RZ,a.INQ,a.W,a.OTQ, b.STNM  from " + ST_RSVR_R + " a," + ST_STBPRP_V + " b where a.STCD=b.STCD ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcd))
            {
                strSqlInnerText += " and a.STCD in (@STCDS)";
                sqlParams.Add("STCDS", stcd);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and a.TM>=@StartDate";
                sqlParams.Add("StartDate", startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and a.TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            //获取每个分组中序号为1的数据，即最新一条数据
            strSqlInnerText += " order by a.TM desc";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlInnerText, sqlParams);
            }
        }

        /// <summary>
        /// 查询水情信息-未分页
        /// add by lw
        /// Date:2019-05-24 17:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<dynamic> GetRsvrData(string stcd, string startDate, string endDate)
        {
            return null;
        }


        /// <summary>
        /// 首页查询8点水库水情过程线信息
        /// add by qlj
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRsvrLineEight(string stcd, string startDate, string endDate)
        {
            //分组排序sql语句
            string strSqlInnerText = "select a.STCD,a.TM, a.RZ,a.INQ,a.W,a.OTQ, b.STNM  from " + ST_RSVR_R + " a,"+ ST_STBPRP_V + " b where a.STCD=b.STCD and CONVERT(VARCHAR(5),a.TM, 24)='08:00' ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcd))
            {
                strSqlInnerText += " and a.STCD in (@STCDS)";
                sqlParams.Add("STCDS", stcd);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and a.TM>=@StartDate";
                sqlParams.Add("StartDate", startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and a.TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            //获取每个分组中序号为1的数据，即最新一条数据
            strSqlInnerText += " order by a.TM desc";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlInnerText, sqlParams);
            }
        }
    }
}
