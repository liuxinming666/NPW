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
using EWF.Entity;
using System.Dynamic;

namespace EWF.Repository
{
	public class PstatRepository : DefaultRepository, IPstatRepository
	{
		private readonly string ST_PPTN_R;
		private readonly string ST_STBPRP_V;
		private readonly string PrimaryTableName;
		public PstatRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			PrimaryTableName = $"{RTDB_Schema}ST_PSTAT_R";
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			ST_PPTN_R = $"{RTDB_Schema}ST_PPTN_R";
		}
		
		public IEnumerable<ST_PSTATEntity> GetDayData(string STCD, string sdate, string edate, int type, string addvcd)
        {
            #region 参数校验
            if (sdate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (edate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            #endregion

            var sqlParams = new DynamicParameters();
			sqlParams.Add(nameof(type), type);
			sqlParams.Add(nameof(addvcd), addvcd);
			sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);

            var strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,ACCP ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd and tbb.TYPE=@type and tbb.ADDVCD=@addvcd ");
			strSql.Append("WHERE (STTDRCD=1) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<@edate) ");

            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_PSTATEntity>(strSql.ToString(), sqlParams);
        }

        public IEnumerable<ST_PSTATEntity> GetDayData_ByPPTN(string STCD, string sdate, string edate, int type, string addvcd)
        {
            #region 参数校验
            if (sdate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (edate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            #endregion

            var sqlParams = new DynamicParameters();
			sqlParams.Add(nameof(type), type);
			sqlParams.Add(nameof(addvcd), addvcd);
			sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);

			var strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,TM AS IDTM, 1 AS STTDRCD, DYP AS ACCP ");
            strSql.Append($"FROM {ST_PPTN_R} tba JOIN  {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd and tbb.TYPE=@type and tbb.ADDVCD=@addvcd ");
			strSql.Append("WHERE (DATEPART(hh,TM)=8) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (TM > @sdate) AND (TM<@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,TM ASC ");

            return database.Connection.Query<ST_PSTATEntity>(strSql.ToString(), sqlParams);
        }
        
        public IEnumerable<ST_PSTATEntity> GetMonthData(string STCD, string sdate, string edate, int type, string addvcd)
        {
            #region 参数校验
            if (sdate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (edate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }

            #endregion

            var sqlParams = new DynamicParameters();
			sqlParams.Add(nameof(type), type);
			sqlParams.Add(nameof(addvcd), addvcd);
			sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);

            var strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,ACCP ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd and tbb.TYPE=@type and tbb.ADDVCD=@addvcd ");
			strSql.Append("WHERE (STTDRCD IN (4,5) ) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_PSTATEntity>(strSql.ToString(), sqlParams);
        }

        public IEnumerable<ST_PSTATEntity> GeYearData(string STCD, string sdate, string edate, int type, string addvcd)
        {
            #region 参数校验
            if (sdate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (edate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
          
            #endregion

            var sqlParams = new DynamicParameters();
			sqlParams.Add(nameof(type), type);
			sqlParams.Add(nameof(addvcd), addvcd);
			sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);

            var strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,ACCP ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd and tbb.TYPE=@type and tbb.ADDVCD=@addvcd ");
			strSql.Append("WHERE (STTDRCD=5) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD) ");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_PSTATEntity>(strSql.ToString(),sqlParams);
        }

        #region 历史同期降水量对比分析
        /// <summary>历史同期降水量对比分析-旬月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="STTDRCD">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="end_history">对比结束日期</param>
        /// <returns>时段内旬月均值</returns>
        public dynamic GetMonthComparativeData(string STCD, string STTDRCD, string sdate, string edate, string sdate_history, string edate_history)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add(nameof(STCD), STCD);
            sqlParams.Add(nameof(STTDRCD), STTDRCD);
            sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);
            sqlParams.Add(nameof(sdate_history), sdate_history);
            sqlParams.Add(nameof(edate_history), edate_history);
            StringBuilder strSql = new StringBuilder();
            //第一条语句
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,cast(round(ACCP,1) as numeric(20,1)) as ACCP ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD=@STTDRCD)");
            strSql.Append("AND (tba.STCD=@STCD)");
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");
            //第二条语句
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,cast(round(ACCP,1) as numeric(20,1)) as ACCP ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD =@STTDRCD)");
            strSql.Append("AND (tba.STCD=@STCD)");
            strSql.Append("AND (idtm >@sdate_history) AND (idtm<=@edate_history) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");


            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.real = multi.Read<ST_PSTATEntity>();
                data.history = multi.Read<ST_PSTATEntity>();
            }

            return data;
        }
        #endregion

    }
}
