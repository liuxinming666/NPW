/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：RiverRepository
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
using System.Dynamic;
using EWF.Entity;
using EWF.Entity.Models;
using System.Data;
using System.Data.SqlClient;

namespace EWF.Repository
{
    public class RiverRepository : DefaultRepository,IRiverRepository
	{
		private readonly string ST_RVAV_R;
		private readonly string ST_RIVER_R;
		private readonly string V_ST_RIVSAND_R;
		private readonly string ST_STBPRP_V;

		public RiverRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			ST_RIVER_R = $"{RTDB_Schema}ST_RIVER_R";
			ST_RVAV_R= $"{RTDB_Schema}ST_RVAV_R";
			V_ST_RIVSAND_R = $"{Default_Schema}V_ST_RIVSAND_R";
		}

        /// <summary>
        /// 获取指定时段内最新水情数据
        /// add by SUN
        /// Date:2019-05-23 10:00
        /// </summary>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetLatestRiver(string startDate, string endDate, string addvcd, string type)
        {
            //分组排序sql语句
            //string strSqlInnerText = "select rtrim(A.RVNM) as RVNM,A.STCD,rtrim(A.STNM) as STNM,B.TM,B.Z,B.Q,B.WPTN,B.S,A.LGTD,A.LTTD,ROW_NUMBER()over(partition by A.STCD order by TM desc) as ROWNUM from ST_STBPRP_B A,[EW_NPW].[dbo].[V_ST_RIVSAND_R] B where A.STCD=B.STCD ";
            //string strSqlInnerText = "select rtrim(A.RVNM) as RVNM,A.STCD,rtrim(A.STNM) as STNM,B.TM,B.Z,B.Q,B.WPTN,B.S,A.LGTD,A.LTTD,ANGLE,ROW_NUMBER()over(partition by A.STCD order by TM desc) as ROWNUM from [EW_NPW].[dbo].[ST_STBPRP_V] A,[EW_NPW].[dbo].[V_ST_RIVSAND_R] B where A.STCD=B.STCD ";
            //string strSqlInnerText = "select rtrim(A.RVNM) as RVNM,A.STCD,rtrim(A.STNM) as STNM,B.TM,B.Z,B.Q,B.WPTN,B.S,A.LGTD,A.LTTD,ROW_NUMBER()over(partition by A.STCD order by TM desc) as ROWNUM from [EW_NPW].[dbo].[ST_STBPRP_V] A,[EW_NPW].[dbo].[V_ST_RIVSAND_R] B where A.STCD=B.STCD ";
            //加上备注超警戒数据
            string strSqlInnerText = "select rtrim(A.RVNM) as RVNM,A.STCD,rtrim(A.STNM) as STNM,B.TM,B.Z,B.Q,B.WPTN,B.FLWCHRCD,B.S,A.LGTD,A.LTTD,ANGLE,ROW_NUMBER()over(partition by A.STCD order by TM desc) as ROWNUM,c.WRZ,c.WRQ, " 
				+ "case when b.z-c.wrz>0 then '水位超警戒'+cast(b.z-c.wrz as varchar) end as remarkz,case when b.q-c.wrq>0 then '流量超警戒' + cast(b.q-c.wrq as varchar) end as remarkq "
                  +
				 $"from {ST_STBPRP_V} A join {V_ST_RIVSAND_R} B on A.STCD=B.STCD left join {RTDB_Schema}st_rvfcch_b c on c.stcd=a.stcd where 1=1 ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and TM>@StartDate";
                //strSqlInnerText += " and TM>CONVERT(datetime, @StartDate, 20)";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and TM<=@EndDate";
                //strSqlInnerText += " and TM<=CONVERT(datetime, @EndDate, 20)";
                sqlParams.Add("EndDate", endDate);
            }
            if (!string.IsNullOrEmpty(addvcd))
            {
                strSqlInnerText += " and A.ADDVCD=@ADDVCD";
                sqlParams.Add("ADDVCD", addvcd);
            }
            if (!string.IsNullOrEmpty(type))
            {
                strSqlInnerText += " and A.TYPE=@TYPE";
                sqlParams.Add("TYPE", type);
            }
            //获取每个分组中序号为1的数据，即最新一条数据
            //string strSqlOuterText = "select RVNM,STCD,STNM,Z,Q,WPTN,TM,S,LTTD,LGTD from ("
            //                         + strSqlInnerText
            //                         + ") aa where ROWNUM=1";
            string strSqlOuterText = "select RVNM,STCD,STNM,Z,Q,WPTN,TM,S,LTTD,LGTD,ANGLE,WRZ,WRQ,FLWCHRCD,isnull(remarkz,'')+isnull(remarkq,'') as REMARK from ("
                                     + strSqlInnerText
                                     + ") aa where ROWNUM=1";
            //string strSqlOuterText = "select RVNM,STCD,STNM,Z,Q,WPTN,TM,S,LTTD,LGTD,ANGLE from ("
            //                         + strSqlInnerText
            //                         + ") aa where ROWNUM=1";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }

        public Page<dynamic> GetReadData(int pageIndex, int pageSize, string STNM)
        {
            //2016-11-24 14:00:00.000

            string sql = @"SELECT tbb.STNM, tba.[STCD],[TM],[FLWCHRCD],[WPTN] ,[MSQMT],[MSAMT],[MSVMT],[XSMXV],[Z],[Q],[XSA],[XSAVV] FROM "+ ST_RIVER_R + " tba,"+ ST_STBPRP_V + " tbb  WHERE "
               +" tba.stcd=tbb.stcd and TM>'2016-10-24 14:00:00.000' ";

            if (!STNM.IsEmpty())
            {
                sql += string.Format(" AND STNM like '%{0}%'", STNM);
            }

            var tableName = ST_RIVER_R + " tba,"+ ST_STBPRP_V + " tbb";
            var flied = "tbb.STNM, tba.[STCD] ,[TM] ,[FLWCHRCD] ,[WPTN]  ,[MSQMT] ,[MSAMT] ,[MSVMT] ,[XSMXV] ,[Z]  ,[Q] ,[XSA] ,[XSAVV]";
            var where = "tba.stcd=tbb.stcd and TM>'2016-10-24 14:00:00.000'";
            var orderby = "TM DESC";
			
            var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby);
            return page;
        }

        /// <summary>
        /// 查询水情信息
        /// </summary>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区、流域编码</param>
        /// <param name="type">行政区、流域类型</param>
        /// <returns></returns>
        public Page<dynamic> GetRiverData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type)
        {
			var field_FLWCHRCD = "(CASE WHEN FLWCHRCD='1' THEN '干枯' WHEN FLWCHRCD='2' THEN '断流' WHEN FLWCHRCD='3' THEN '流向不定' WHEN FLWCHRCD='4' THEN '逆流' WHEN FLWCHRCD='5' THEN '起涨' WHEN FLWCHRCD='6' THEN '<FONT COLOR=RED>洪峰</FONT>' WHEN FLWCHRCD='P' THEN '水电厂发电流量' ELSE '' END ) AS FLWCHRCD";
			var field_WPTN = "(CASE WHEN WPTN = '4' THEN '<font color=green>落</font>' WHEN WPTN = '5' THEN '<font color=red>涨</font>' WHEN WPTN = '6' THEN '平' ELSE ' ' END) AS WPTN";
			var field_MSQMT = "(CASE WHEN MSQMT = '1' THEN '水位流量关系曲线' WHEN MSQMT = '2' THEN '浮标及溶液测流法' WHEN MSQMT = '3' THEN '流速仪及量水建筑物'  WHEN MSQMT = '4' THEN '估算法'  WHEN MSQMT = '5' THEN 'ADCP'  WHEN MSQMT = '6' THEN '电功率反推法' WHEN MSQMT = '9' THEN '其他方法' ELSE '' END) AS MSQMT";
			
			var fields = $"RVNM,STNM,  STCD,TM,Z,Q,XSA,{field_FLWCHRCD},{field_WPTN},{field_MSQMT},  S,AVZ,AVQ";
			var tableName = Default_Schema + "[V_ST_RIVSAVQ_R]";
            var orderby = "TM DESC";
			var where = "1=1 ";

			//参数列表
			var sqlParams = new DynamicParameters();
			if (!string.IsNullOrEmpty(stcds))
			{
				where += " and STCD in (" + stcds + ")";
				//sqlInnerText += " and A.STCD in @STCDS";
				sqlParams.Add("STCDS", stcds.Split(','));
			}
			else
			{
				//where += " and TYPE='" + type + "' and ADDVCD='" + addvcd + "'";
				//where += " and TYPE=@TYPE and ADDVCD=@ADDVCD";

				where += $" and STCD in (select tz.stcd from ST_ADDVCD_POINT tz where tz.type='{type}' and tz.addvcd='{addvcd}') ";
				//where += $" and STCD in (select tz.stcd from ST_ADDVCD_POINT tz where and TYPE=@TYPE and ADDVCD=@ADDVCD) ";
				
				sqlParams.Add("TYPE", type);
				sqlParams.Add("ADDVCD", addvcd);
			}
			
			if (!string.IsNullOrEmpty(startDate))
			{
				
				where += " and TM>'" + startDate + "'";
				//sqlInnerText += " and TM>@StartDate";
				sqlParams.Add("StartDate", startDate);
			}

			if (!string.IsNullOrEmpty(endDate))
			{
				where += " and TM<='" + endDate + "'";
				//sqlInnerText += " and TM<=@EndDate";
				sqlParams.Add("EndDate", endDate);
			}

			var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, fields, where, orderby,null);
			//var page = database.GetPagination_Join<dynamic>(pageIndex, pageSize, tableName, fields, where, orderby);
			return page;
        }

        /// <summary>
        /// 查询首页单块单站水情信息
        /// </summary>
        ///<param name="unit">所属单位</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRiverData(string unit, string stnm)
        {
			string sqlConfigStcd = $"select replace(c.stcd,',',''',''') as stcds from {Default_Schema}TBL_SYS_SYSCONFIG C where c.ADDVCD='" + unit + "'";
            ////获取水位、流量、含沙量
            //string sqlInnerText = "select A.RVNM,A.STCD,A.STNM,B.TM,(CASE WHEN B.FLWCHRCD='1' THEN '干枯' WHEN B.FLWCHRCD='2' THEN '断流' WHEN B.FLWCHRCD='3' THEN '流向不定' WHEN B.FLWCHRCD='4' THEN '逆流' WHEN B.FLWCHRCD='5' THEN '起涨' WHEN B.FLWCHRCD='6' THEN '<FONT COLOR=RED>洪峰</FONT>' WHEN B.FLWCHRCD='P' THEN '水电厂发电流量' ELSE '' END ) AS FLWCHRCD "
            //                     + " , B.Z,(CASE WHEN B.WPTN = '4' THEN '<font color=green>落</font>' WHEN B.WPTN = '5' THEN '<font color=red>涨</font>' WHEN B.WPTN = '6' THEN '平' ELSE ' ' END) AS WPTN, B.Q "
            //                     + " ,(CASE WHEN B.MSQMT = '1' THEN '水位流量关系曲线' WHEN B.MSQMT = '2' THEN '浮标及溶液测流法' WHEN B.MSQMT = '3' THEN '流速仪及量水建筑物'  WHEN B.MSQMT = '4' THEN '估算法'  WHEN B.MSQMT = '5' THEN 'ADCP'  WHEN B.MSQMT = '6' THEN '电功率反推法' WHEN B.MSQMT = '9' THEN '其他方法' ELSE '' END) AS MSQMT "
            //                     + " , B.XSA,B.S from EW_NPW.dbo.ST_STBPRP_V A, [EW_NPW].[dbo].[V_ST_RIVSAND_R] B,EW_NPW.dbo.TBL_SYS_SYSCONFIG C where A.STCD = B.STCD AND B.STCD=C.STCD AND TM>'" + DateTime.Now.AddDays(-10) + "'";

            //获取水位、流量、含沙量
            string sqlInnerText = "select A.RVNM,A.STCD,A.STNM,B.TM,(CASE WHEN B.FLWCHRCD='1' THEN '干枯' WHEN B.FLWCHRCD='2' THEN '断流' WHEN B.FLWCHRCD='3' THEN '流向不定' WHEN B.FLWCHRCD='4' THEN '逆流' WHEN B.FLWCHRCD='5' THEN '起涨' WHEN B.FLWCHRCD='6' THEN '<FONT COLOR=RED>洪峰</FONT>' WHEN B.FLWCHRCD='P' THEN '水电厂发电流量' ELSE '' END ) AS FLWCHRCD "
                                 + " , B.Z,(CASE WHEN B.WPTN = '4' THEN '<font color=green>落</font>' WHEN B.WPTN = '5' THEN '<font color=red>涨</font>' WHEN B.WPTN = '6' THEN '平' ELSE ' ' END) AS WPTN, B.Q "
                                 + " ,(CASE WHEN B.MSQMT = '1' THEN '水位流量关系曲线' WHEN B.MSQMT = '2' THEN '浮标及溶液测流法' WHEN B.MSQMT = '3' THEN '流速仪及量水建筑物'  WHEN B.MSQMT = '4' THEN '估算法'  WHEN B.MSQMT = '5' THEN 'ADCP'  WHEN B.MSQMT = '6' THEN '电功率反推法' WHEN B.MSQMT = '9' THEN '其他方法' ELSE '' END) AS MSQMT "
                                 + $" , B.XSA,B.S from {ST_STBPRP_V} A, {V_ST_RIVSAND_R} B where A.STCD = B.STCD AND TM>'" + DateTime.Now.AddDays(-10) + "'";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(unit))
            {
                sqlInnerText += " and A.ADDVCD=@unit";
                sqlParams.Add("unit", unit);
            }
            if (!string.IsNullOrEmpty(stnm))
            {
                sqlInnerText += " and A.STNM=@stnm";
                sqlParams.Add("stnm", stnm);
            }
            DataTable dtStcd = new DataTable();
            dtStcd = database.FindTable(sqlConfigStcd);
            string stcds = "";
            stcds = "'" + dtStcd.Rows[0]["stcds"].ToString() + "'";
            sqlInnerText += " and B.STCD IN(" + stcds + ")";
            sqlInnerText += " ORDER BY a.STCD, TM DESC";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }

        /// <summary>
        /// 查询水情信息-未分页
        /// add by SUN
        /// Date:2019-05-23 17:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="hourFilter">小时过滤(整点过滤，范围-1-23，-1表示不过滤)，默认只显示8点数据  eg:传入6,则只显示6点数据</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRiverData(string stcd, string startDate, string endDate,int hourFilter=8)
        {
            //分组排序sql语句
            string strSqlInnerText = $"select A.STCD,A.STNM,B.TM,B.Z,B.Q,B.FLWCHRCD,B.WPTN,B.MSQMT,B.XSA,B.S from {ST_STBPRP_V} A,{V_ST_RIVSAND_R} B where A.STCD=B.STCD ";
            if (hourFilter < -1 || hourFilter >= 24)
            {
                throw new Exception("hourFilter参数，hourFilter数值范围应在-1-23之内,-1表示不过滤时间！");
            }
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcd))
            {
                strSqlInnerText += " and A.STCD=@STCD ";
                sqlParams.Add("STCD", stcd);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and TM>@StartDate ";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            
            if (hourFilter != -1)
            {
                string strHourtFilter = "";
                strSqlInnerText += " and CONVERT(varchar(5) ,TM, 108 )=@HourFilter";
                if (hourFilter < 10)
                {
                    strHourtFilter = "0" + hourFilter+":00";
                }
                else
                {
                    strHourtFilter = hourFilter.ToString()+":00";
                }
                sqlParams.Add("HourFilter", strHourtFilter);
            }
			//获取每个分组中序号为1的数据，即最新一条数据
			string strSqlOuterText = "select  AA.STCD,AA.STNM,AA.TM,AA.Z,AA.Q,AA.FLWCHRCD,AA.WPTN,AA.MSQMT,AA.XSA,AA.S,BB.WRZ,BB.WRQ  from ("
									 + strSqlInnerText
									 + ") AA left join " + RTDB_Schema + "ST_RVFCCH_B BB on AA.STCD=BB.STCD order by TM desc";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }

        /// <summary>
        /// 查询多站水情数据-未分页
        /// add by SUN
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRiverDataByMultiStcds(string stcds, string startDate, string endDate)
        {
			var sqlText = $"select A.RVNM,A.STCD,A.STNM,A.LGTD,A.LTTD,A.ADDVCD,A.STTP,B.TM,B.Z,B.Q from {ST_STBPRP_V} A, {V_ST_RIVSAND_R} B where A.STCD=B.STCD ";
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcds))
            {
                sqlText += " and A.STCD in @STCDS ";
                sqlParams.Add("STCDS", stcds.Split(','));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlText += " and TM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlText += " and TM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            sqlText += " order by TM";
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlText, sqlParams);
            }
        }


        /// <summary>
        /// 查询河道水情均值
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRvavData(string stcd, string startDate, string endDate)
        {
            string sqlText = $"select A.RVNM,A.STCD,A.STNM,A.ADDVCD,A.STTP,A.LGTD,A.LTTD,B.IDTM,B.AVZ,B.AVQ from {ST_STBPRP_V} A, {ST_RVAV_R} B where A.STCD=B.STCD and sttdrcd=1";
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcd))
            {
                sqlText += " and A.STCD=@STCD";
                sqlParams.Add("STCD", stcd);
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlText += " and idtm>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlText += " and idtm<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlText, sqlParams);
            }
        }

        #region 历史同期对比水位、流量
        public dynamic GetMutiRiverData_Comparative(string stcds, string startDate, string endDate, string startDate_history, string endDate_history)
        {
            #region 参数校验
            if (stcds.IsEmpty())
            {
                throw new ArgumentNullException("测站编码");
            }
            if (startDate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (endDate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            if (startDate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            if (endDate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("STCD", stcds.Split(','));
            sqlParams.Add("sdate", startDate);
            sqlParams.Add("edate", endDate);
            sqlParams.Add("state_history", startDate_history);
            sqlParams.Add("edate_history", endDate_history);

            //第一条语句 当前的水位流量
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select a.STCD,tbb.STNM,a.TM,a.Z,a.Q from {ST_RIVER_R} a left join {RTDB_Schema}ST_STBPRP_B tbb ON a.stcd = tbb.stcd");
            strSql.Append(" where a.stcd in @STCD and a.TM > @sdate and a.TM < @edate and Datename(hour, a.TM) = 8 and  Datename(minute, a.TM) = 0 and Datename(second, a.TM) = 0 order by a.STCD, a.TM asc;");

            //第二条语句 历史的水位流量
            strSql.Append($"select a.STCD,tbb.STNM,a.TM,a.Z,a.Q from {ST_RIVER_R} a left join {RTDB_Schema} ST_STBPRP_B tbb ON a.stcd = tbb.stcd");
            strSql.Append(" where a.stcd in @STCD and a.TM > @state_history and a.TM < @edate_history and Datename(hour, a.TM) = 8 and  Datename(minute, a.TM) = 0 and Datename(second, a.TM) = 0 order by a.STCD, a.TM asc;");
            
            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.riverreal = multi.Read<ST_RIVEREntity>();
                data.riverhistory = multi.Read<ST_RIVEREntity>();
            }

            return data;
        }
        #endregion

        #region 历史同期对比含沙量
        public dynamic GetMutiSedData_Comparative(string stcds, string startDate, string endDate, string startDate_history, string endDate_history)
        {
            #region 参数校验
            if (stcds.IsEmpty())
            {
                throw new ArgumentNullException("测站编码");
            }
            if (startDate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (endDate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            if (startDate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            if (endDate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("STCD", stcds.Split(','));
            sqlParams.Add("sdate", startDate);
            sqlParams.Add("edate", endDate);
            sqlParams.Add("state_history", startDate_history);
            sqlParams.Add("edate_history", endDate_history);

            //第一条语句 当前的含沙量
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select a.STCD,tbb.STNM,a.TM,a.S from  {RTDB_Schema}ST_SED_R a left join {RTDB_Schema}ST_STBPRP_B tbb ON a.stcd = tbb.stcd");
            strSql.Append(" where a.stcd in @STCD and a.TM > @sdate and a.TM<@edate and Datename(hour,a.TM) = 8 and  Datename(minute,a.TM) = 0 and Datename(second,a.TM) = 0 order by a.STCD,a.TM asc;");
            //第二条语句 历史含沙量
            strSql.Append($"select a.STCD,tbb.STNM,a.TM,a.S from  {RTDB_Schema}ST_SED_R a left join {RTDB_Schema}ST_STBPRP_B tbb ON a.stcd = tbb.stcd");
            strSql.Append(" where a.stcd in @STCD and a.TM > @state_history and a.TM<@edate_history and Datename(hour,a.TM) = 8 and  Datename(minute,a.TM) = 0 and Datename(second,a.TM) = 0 order by a.STCD,a.TM asc;");

            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.sedreal = multi.Read<ST_SEDEntity>();
                data.sedhistory = multi.Read<ST_SEDEntity>();
            }

            return data;
        }
        #endregion


        #region 水流沙过程对照
        public dynamic GetMutliStationZQS(string stcds, string stime, string etime)
        {
            #region 参数校验
            if (stcds.IsEmpty())
            {
                throw new ArgumentNullException("测站编码");
            }
            if (stime.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (etime.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("STCD", stcds.Split(','));
            sqlParams.Add("sdate", stime);
            sqlParams.Add("edate", etime);

            //第一条语句 水位流量
            var strSql = new StringBuilder();
            strSql.Append("select a.STCD,tbb.STNM,a.TM,a.Z,a.Q,a.MSQMT from " + ST_RIVER_R + " a left join " + RTDB_Schema + "ST_STBPRP_B tbb ON a.stcd = tbb.stcd");
            strSql.Append(" where a.stcd in @STCD and a.TM > @sdate and a.TM < @edate order by a.STCD, a.TM asc;");

            //第二条语句 含沙量
            strSql.Append("select a.STCD,tbb.STNM,a.TM,a.S from  " + RTDB_Schema + "ST_SED_R a left join " + RTDB_Schema + "ST_STBPRP_B tbb ON a.stcd = tbb.stcd");
            strSql.Append(" where a.stcd in @STCD and a.TM > @sdate and a.TM<@edate  order by a.STCD,a.TM asc;");

            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.zqlist = multi.Read<ST_RIVEREntity>();
                data.slist = multi.Read<ST_RIVEREntity>();
            }

            return data;
        }
        #endregion

        #region 断面水位
        /// <summary>
        /// 断面水位数据
        /// add by SUN
        /// Date:2019-07-10 11:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="stnm"></param>
        /// <param name="tm"></param>
        /// <param name="sDt"></param>
        /// <returns></returns>
        public DataSet GetSectionZ(string stcd, string stnm, string tm, string sDt)
        {
            if (string.IsNullOrEmpty(stcd) || string.IsNullOrEmpty(stnm) || string.IsNullOrEmpty(tm) || string.IsNullOrEmpty(sDt))
            {
                throw new ArgumentNullException();
            }

            string strSql = $"select a.STCD,a.SCSJ,QDJ,GC from {File_Schema}TBL_SCDM_SWZ a,(select max(SCSJ) as SCSJ, STCD, MS  from {File_Schema}TBL_SCDM_SWZ aa where GC >= 0 and STCD = '"
                        +stcd+"' and rtrim(MS) = '"+stnm+"' group by STCD, MS) b where a.SCSJ = B.SCSJ and a.STCD = b.STCD and a.MS = b.MS order by QDJ";
            DataSet ds = new DataSet();
           
            DataTable dtSection = database.FindTable(strSql);
            dtSection.TableName = "dtSection"; 
            string sqlZ = $"select a.STCD,TM,Z,Q from {Default_Schema}[V_ST_RIVSAND_R] a where a.stcd='" + stcd+"' and tm>'"+DateTime.Parse(tm).AddDays(-7).ToString()+"' and tm<='"+tm+"' order by tm";
            DataTable dtZ = database.FindTable(sqlZ);
            dtZ.TableName = "dtZ";
            string sqlWarnZ = $"select STCD,WRZ from {RTDB_Schema}ST_RVFCCH_B where stcd='" + stcd + "'";
            DataTable dtWarnZ = database.FindTable(sqlWarnZ);
            dtWarnZ.TableName = "dtWarnZ";
            ds.Tables.Add(dtSection);
            ds.Tables.Add(dtZ);
            ds.Tables.Add(dtWarnZ);
            return ds;
        }
        #endregion
    }
}
