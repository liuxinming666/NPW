/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianPing
 * 日  期：2019/5/27 14:36:04
 * 描  述：RainRepository
 * *********************************************/
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Util;
using EWF.Util.Page;
using System.Data;

namespace EWF.Repository
{
    public class RainRepository :DefaultRepository, IRainRepository
    {
		private readonly string ST_PPTN_R;
		private readonly string ST_STBPRP_V;
		public RainRepository(IOptionsSnapshot<DbOption> options) : base(options) 
        {
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			ST_PPTN_R = $"{RTDB_Schema}ST_PPTN_R";
		}

		/// <summary>
		/// 获取时段雨量数据
		/// add by SUN
		/// Date:2019-08-03 14:00
		/// </summary>
		/// <param name="stcds">站码（为空则查询全部，非空格式为：'44001050','44001800'）</param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="addvcd"></param>
		/// <returns></returns>
		public DataTable GetRainDataPeriod(string stcds, string startDate, string endDate, string addvcd)
		{
			var sqlText = $"select b.BSNM,b.RVNM,b.STCD,b.STNM,b.LGTD,b.LTTD,a.TM,a.DRP,a.DYP,a.WTH from  {ST_PPTN_R} a,{ST_STBPRP_V}  b where a.stcd=b.stcd ";

			if (!string.IsNullOrEmpty(stcds))
			{
				sqlText += " and b.STCD in( " + stcds + ") ";
			}
			if (string.IsNullOrEmpty(startDate))
			{

				throw new ArgumentNullException("startDate", "起始时间不能为空！");
			}
			if (string.IsNullOrEmpty(endDate))
			{
				throw new ArgumentNullException("endDate", "结束时间不能为空！");
			}
			sqlText += " and tm > '" + startDate + "' ";
			sqlText += " and tm<='" + endDate + "' ";
			if (!string.IsNullOrEmpty(addvcd))
			{
				sqlText += " and addvcd = '" + addvcd + "'";
			}
			sqlText += " order by b.stcd,tm";

			return database.FindTable(sqlText);
		}


		/// <summary>
		/// 获取每个站的最新降雨
		/// </summary>
		/// <returns></returns>
		public IEnumerable<dynamic> GetLatestRain(string startDate, string endDate, int type, string addvcd)
        {
            string strSql = "SELECT a.[STCD],rtrim(stnm) as STNM,a.[TM],[WTH],[DRP],[INTV],[PDR],[DYP],LGTD,LTTD FROM " + RTDB_Schema + "[ST_PPTN_R] a,(select stcd, max(tm) as tm from " + RTDB_Schema + "[ST_PPTN_R] WHERE DRP>0 ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql += " and TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            strSql += $" group by stcd) b, {ST_STBPRP_V} c where c.stcd=a.stcd and a.stcd= b.stcd and c.TYPE="+type+" and c.ADDVCD = '"+addvcd+"' and a.tm= b.tm";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql, sqlParams);
            }
        }
        /// <summary>
        /// 查询多日雨情信息By JinJianping
        /// </summary>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRainData(string stcds, string startDate, string endDate, int type, string addvcd)
        {
            startDate = startDate + " 00:00";
            endDate = endDate + " 23:59";
            //获取多日雨情
            //string sqlInnerText = "select a.STCD,a.TM,a.DYP,b.STNM from ST_PPTN_R a left join ST_STBPRP_B b on a.STCD=b.STCD where a.TM >='2019-5-01 08:00' and a.TM<='2019-5-29 08:00' and a.STCD in ('40101200','40101600') order by a.STCD,a.TM desc";
            string sqlInnerText = "select * from " + RTDB_Schema + "ST_PPTN_R a where 1=1  and dyp is not null";
            //参数列表
            var sqlParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(stcds))
            {
                sqlInnerText += " and A.STCD in @STCDS";
                sqlParams.Add("STCDS", stcds.Split(','));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            string strSqlOuterText = "select AA.STCD,AA.TM,AA.DYP,BB.STNM from ("
                                     + sqlInnerText
                                     + ") AA inner join  "+ST_STBPRP_V+" BB on AA.STCD=BB.STCD where BB.TYPE=" + type + " and BB.ADDVCD = '" + addvcd + "' and BB.STNM is not null order by AA.STCD ASC,TM asc";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }
        /// <summary>
        /// 统计各站点一段时间内的降雨量
        /// add by SUN
        /// Date:2019-05-27 15:00
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSumRain(string startDate, string endDate, int areatype, string addvcd, int type = 1)
        {
            //判断type字段传入值是否合法
            if (type != 0 && type != 1)
            {
                throw new Exception("type参数只支持0,1两个值，0表示时段雨量，1表示日雨量");
            }

            string sqlText = string.Empty;
            //统计时段雨量
            if (type == 0)
            {
                sqlText = "select RVNM,a.STCD,a.STNM,a.LGTD as x,a.LTTD as y,sum(b.DRP) as value,'" + startDate + "' as STM,'" + endDate + "' as ETM from "+ST_STBPRP_V+" a," + RTDB_Schema + "st_pptn_r b where a.stcd=b.stcd and a.TYPE=" + areatype + " and a.ADDVCD='" + addvcd + "' and lgtd is not null ";
            }
            else
            {
                sqlText = "select RVNM,a.STCD,a.STNM,a.LGTD as x,a.LTTD as y,sum(b.DYP) as value,'" + startDate + "' as STM,'" + endDate + "' as ETM from "+ST_STBPRP_V+" a," + RTDB_Schema + "st_pptn_r b where a.stcd=b.stcd and a.TYPE=" + areatype + " and a.ADDVCD='" + addvcd + "' and lgtd is not null ";
            }

            //参数化对象
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(startDate))
            {
                sqlText += " and b.TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlText += " and b.TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            //过滤掉经纬度一样的站，避免同一经纬度有多个不同的值
            sqlText += " and not exists (select 1 from " + ST_STBPRP_V + " where a.LGTD = LGTD and a.LTTD = LTTD and STCD > a.STCD)";
            sqlText += " group by rvnm,a.stcd,a.stnm,a.lgtd,a.lttd";
            if(type == 0)
                sqlText += " order by sum(b.DRP) desc,a.stcd";
            else
                sqlText += " order by sum(b.DYP) desc,a.stcd";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlText, sqlParams);
            }
        }
        /// <summary>
        /// 查询时段累积雨量
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public Page<dynamic> GetRainDataPeriod(int pageIndex, int pageSize, string stcds, string startDate, string endDate, int type, string addvcd)
        {


            //获取时段内累积雨量值
            string sqlInnerText = "select STCD,SUM(DRP) AS VALUE from " + RTDB_Schema + "ST_PPTN_R  where 1=1 ";
            //参数列表
            var sqlParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(stcds))
            {
                sqlInnerText += " and STCD in @STCDS";
                sqlParams.Add("STCDS", stcds.Split(','));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }

            sqlInnerText += " GROUP BY STCD ";
            //var tableName = "(" + sqlInnerText + ") AA left join ST_STBPRP_B BB on AA.STCD=BB.STCD ";
            var tableName = "(" + sqlInnerText + ") AA inner join  "+ST_STBPRP_V+" BB on AA.STCD=BB.STCD and BB.TYPE="+type+" and BB.ADDVCD='"+addvcd+"'";
            var flied = "BB.RVNM,BB.STCD,BB.STNM,CONVERT(numeric(8,1),round(AA.VALUE,1)) AS VALUE";
            var where = "1=1";
            var orderby = "BB.RVNM DESC";

            //var db = new RepositoryBase(database);
            var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby, sqlParams);

            return page;

        }
        /// <summary>
        /// 获取区域的面平均雨量，根据权重计算
        /// add by SUN
        /// Date:2019-05-30 13:00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="featureType">要素类型（0-时段雨量DRP,1-日雨量DYP)</param>
        /// <returns></returns>
        public double GetAreaAvg(string startDate, string endDate, string adcd, int featureType=1)
        {

            string field = "";
            if (featureType != 0 && featureType != 1)
            {
                throw new Exception("参数赋值不对，featureType字段只能为0或1，0-时段雨量DRP,1-日雨量DYP");
            }
            if (featureType == 0)
            {
                field = "DRP";
            }
            if (featureType == 1)
            {
                field = "DYP";
            }
            string sqlAreaAvg = "select sum(a."+ field + "*b.WEIGHT) as AREAAVG from " + RTDB_Schema + "st_pptn_r a,"+Default_Schema+"TBL_AREA b where a.STCD=b.STCD ";

            //参数列表
            var sqlParams = new DynamicParameters();


            if (string.IsNullOrEmpty(startDate))
            {
                throw new Exception("参数起始日期startDate不能为空！");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                throw new Exception("参数结束日期endDate不能为空！");
            }

            sqlAreaAvg += " and TM>@StartDate";
            sqlParams.Add("StartDate", startDate);

            sqlAreaAvg += " and TM<=@EndDate";
            sqlParams.Add("EndDate", endDate);

            if (!string.IsNullOrEmpty(adcd))
            {
                sqlAreaAvg += " and ADCD=@Adcd";
                sqlParams.Add("Adcd", adcd);
            }
            using (var areaConn = database.Connection)
            {
                return areaConn.ExecuteScalar<double>(sqlAreaAvg, sqlParams);

            }
        }
        /// <summary>
        /// 查询旬月雨情信息By JinJianping
        /// </summary>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRainMonthData(string stcds, string startDate, string endDate, int type, string addvcd)
        {
            startDate = startDate + "-1 0:0:0";
            endDate = endDate + "-1 8:0:0";
            //获取多日雨情
            string sqlInnerText = $"select * from {RTDB_Schema}ST_PSTAT_R a where (STTDRCD=4 or STTDRCD=5)";//4是旬 5是月
            //参数列表
            var sqlParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(stcds))
            {
                sqlInnerText += " and A.STCD in @STCDS";
                sqlParams.Add("STCDS", stcds.Split(','));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and IDTM>@StartDate";
                sqlParams.Add("StartDate", Convert.ToDateTime(startDate).ToString("yyyy-MM")+"-11 0:0:0");
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and IDTM<=@EndDate";
                sqlParams.Add("EndDate", Convert.ToDateTime(endDate).AddMonths(1).ToString("yyyy-MM") + "-01 8:0:0");
            }
            //string strSqlOuterText = "select AA.STCD,AA.IDTM,AA.STTDRCD,AA.ACCP,BB.STNM from ("
            //                         + sqlInnerText
            //                         + ") AA left join ST_STBPRP_B BB on AA.STCD=BB.STCD where BB.STNM is not null order by AA.STCD,IDTM asc";
            string strSqlOuterText = "select AA.STCD,AA.IDTM,AA.STTDRCD,AA.ACCP,BB.STNM from ("
                                    + sqlInnerText
                                    + ") AA inner join  "+ST_STBPRP_V+" BB on AA.STCD=BB.STCD where BB.TYPE="+type+" and BB.ADDVCD = '"+addvcd+"' and BB.STNM is not null order by AA.STCD,IDTM asc";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }

        public IEnumerable<dynamic> GetRainDataPeriodDetail(string stcd, string startDate, string endDate)
        {
            string sqlInnerText = "select B.RVNM,B.STCD,B.STNM,A.TM,CONVERT(numeric(8,1),round(A.DRP,1)) AS DRP from " + RTDB_Schema + "ST_PPTN_R A," + RTDB_Schema + "ST_STBPRP_B B WHERE A.STCD =B.STCD  and A.DRP is not null ";
            //参数列表
            var sqlParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(stcd))
            {
                sqlInnerText += " and A.STCD = @STCD";
                sqlParams.Add("@STCD", stcd);
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            sqlInnerText += " ORDER BY TM DESC";
            using (var db = database.Connection)
            {
                return db.Query(sqlInnerText, sqlParams);
            }

        }
        /// <summary>
        /// 获取雨强信息
        /// add by SUN
        /// Date:2019-05-30 10:00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sect">时段长（小时）</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRaininess(string startDate, string endDate, int type, string addvcd, int sect = 1)
        {
            var sqlParams = new DynamicParameters();
            if (string.IsNullOrEmpty(startDate))
            {
                throw new Exception("起始日期不能为空！");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                throw new Exception("结束日期不能为空！");
            }
            sqlParams.Add("@stime", startDate);
            sqlParams.Add("@etime", endDate);
            sqlParams.Add("@type", type);
            sqlParams.Add("@addvcd", addvcd);
            sqlParams.Add("@sect", sect);
            using (var db = database.Connection)
            {
                return db.Query(Default_Schema+"[P_RainSta_Extremum]", sqlParams, null, true, null, System.Data.CommandType.StoredProcedure);
            }

        }
        /// <summary>
        /// 获取首页雨量柱状图信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="stype">0：时段降雨DRP  1：昨日降雨DYP</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRainChartData(string stcd, string startDate, string endDate, string stype)
        {
            string sqlInnerText = "";
            if (stype == "0")
            {
                sqlInnerText = "select B.RVNM,B.STCD,B.STNM,A.TM,CONVERT(numeric(8,1),round(A.DRP,1)) AS DRP from " + RTDB_Schema + "ST_PPTN_R A," + RTDB_Schema + "ST_STBPRP_B B WHERE A.STCD =B.STCD  and A.DRP is not null ";
            }
            else
            {
                sqlInnerText = "select B.RVNM,B.STCD,B.STNM,A.TM,CONVERT(numeric(8,1),round(A.DYP,1)) AS DRP from " + RTDB_Schema + "ST_PPTN_R A," + RTDB_Schema + "ST_STBPRP_B B WHERE A.STCD =B.STCD  and A.DYP is not null ";
            }
                //参数列表
            var sqlParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(stcd))
            {
                sqlInnerText += " and A.STCD = @STCD";
                sqlParams.Add("@STCD", stcd);
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            sqlInnerText += " ORDER BY TM DESC";
            using (var db = database.Connection)
            {
                return db.Query(sqlInnerText, sqlParams);
            }

        }

        /// <summary>
        /// 首页获取最新24小时降雨量统计
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">1行政区 2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetLatestRainStaticData(string startDate, string endDate, int type, string addvcd)
        {
            string sqlInnerText = "select c.stcd,sum(case when c.drp is null then 0 else c.drp end) as drp FROM " + RTDB_Schema + "[ST_PPTN_R] c,"

				+ST_STBPRP_V + " d where c.stcd = d.stcd and d.type=" + type + " and d.addvcd='" + addvcd + "' and c.drp>0 ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(startDate))
            {
                sqlInnerText += " and c.TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlInnerText += " and c.TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            sqlInnerText += " group by c.stcd";

            string strSql = "select (case when drp >=250 then '>=250' when drp >=100 then '100~250' when drp >= 50 then '50~100' when drp >=25 then '25~50' "
                + " when drp >= 10 then '10~25' when drp > 0 then '0~10' end ) as SLEVEL,count(*) as SCOUNT,(case when drp >=250 then '5' when drp >=100 then '4' when drp >= 50 then '3' "
                +" when drp >= 25 then '2' when drp >= 10 then '1' when drp > 0 then '0' end ) as SSORT,'" + endDate + "' as TM "
                + " from (" + sqlInnerText + ")a ";
            strSql += " group by (case when drp >=250 then '>=250' when drp >=100 then '100~250' when drp >= 50 then '50~100' when drp >= 25 then '25~50' when drp >= 10 then '10~25' "
                + " when drp > 0 then '0~10' end ),(case when drp >= 250 then '5' when drp >= 100 then '4' when drp >= 50 then '3' "
                + " when drp >= 25 then '2' when drp >= 10 then '1' when drp > 0 then '0' end ) ";

            strSql += " order by (case when drp >= 250 then '5' when drp >= 100 then '4' when drp >= 50 then '3' when drp >= 25 then '2' when drp >= 10 then '1' when drp > 0 then '0' end ) desc";

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql, sqlParams);
            }
        }

        /// <summary>
        /// 首页获取最新24小时降雨量统计数量明细
        /// </summary>
        /// <param name="slevel">降雨量级别</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">1行政区 2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRainStaticDetailData(string slevel, string startDate, string endDate, int type, string addvcd)
        {
            string strSql = "select d.rvnm, c.stcd,d.stnm,sum(case when c.drp is null then 0 else c.drp end) as drp FROM " + RTDB_Schema + "[ST_PPTN_R] c,"
                + ST_STBPRP_V + " d where c.stcd = d.stcd and d.type=" + type + " and d.addvcd='" + addvcd + "' and c.drp>0 ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql += " and c.TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " and c.TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            strSql += " group by d.rvnm,c.stcd,d.stnm ";

            if (slevel == "0~10")
                strSql += " having sum(case when c.drp is null then 0 else c.drp end)>0 and sum(case when c.drp is null then 0 else c.drp end)<10";
            else if (slevel == "10~25")
                strSql += " having sum(case when c.drp is null then 0 else c.drp end)>=10 and sum(case when c.drp is null then 0 else c.drp end)<25";
            else if (slevel == "25~50")
                strSql += " having sum(case when c.drp is null then 0 else c.drp end)>=25 and sum(case when c.drp is null then 0 else c.drp end)<50";
            else if (slevel == "50~100")
                strSql += " having sum(case when c.drp is null then 0 else c.drp end)>=50 and sum(case when c.drp is null then 0 else c.drp end)<100";
            else if (slevel == "100~250")
                strSql += " having sum(case when c.drp is null then 0 else c.drp end)>=100 and sum(case when c.drp is null then 0 else c.drp end)<250";
            else if(slevel == ">=250")
                strSql += " having sum(case when c.drp is null then 0 else c.drp end)>=250";
            strSql += " order by drp desc,c.stcd";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql, sqlParams);
            }
        }

    }
}
