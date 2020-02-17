/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/25 16:46:14
 * 描  述：SedrfRepository
 * *********************************************/
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.IRepository;
using EWF.Util;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace EWF.Repository
{
    public class SedrfRepository : DefaultRepository,ISedrfRepository
	{
		private readonly string ST_STBPRP_V;
		private readonly string PrimaryTableName;
		public SedrfRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			PrimaryTableName = $"{RTDB_Schema}ST_SEDRF_R";
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
		}

		public IEnumerable<ST_SEDRFEntity> GetDayData(string STCD, string addvcd, string type, string sdate, string edate)
        {
            #region 参数校验
            if (sdate.IsEmpty())
                throw new ArgumentNullException(nameof(sdate), "开始日期");
            if (edate.IsEmpty())
                throw new ArgumentNullException(nameof(edate), "结束日期");
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,WRNF,STW ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V} where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD=1) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<@edate) ");

            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_SEDRFEntity>(strSql.ToString(), sqlParams);
        }
        
        public IEnumerable<ST_SEDRFEntity> GetMonthData(string STCD, string addvcd, string type, string sdate, string edate)
        {
            #region 参数校验
            if (sdate.IsEmpty())
                throw new ArgumentNullException(nameof(sdate), "开始日期");
            if (edate.IsEmpty())
                throw new ArgumentNullException(nameof(edate), "结束日期");
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,WRNF,STW ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD IN (4,5) ) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_SEDRFEntity>(strSql.ToString(), sqlParams);
        }

        public IEnumerable<ST_SEDRFEntity> GeYearData(string STCD, string addvcd, string type, string sdate, string edate)
        {
            #region 参数校验
            if (sdate.IsEmpty())
                throw new ArgumentNullException(nameof(sdate), "开始日期");
            if (edate.IsEmpty())
                throw new ArgumentNullException(nameof(edate), "结束日期");
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,WRNF,STW ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD=5)");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_SEDRFEntity>(strSql.ToString(), sqlParams);
        }


        public dynamic GetDayData_Comparative(string STCD, string addvcd, string type, string sdate, string edate, string state_history, string edate_history)
        {
            #region 参数校验
            if (STCD.IsEmpty())
            {
                throw new ArgumentNullException("测站编码");
            }
            if (sdate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (edate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            if (state_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            if (edate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add("STCD", STCD);
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);
            sqlParams.Add("state_history", state_history);
            sqlParams.Add("edate_history", edate_history);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);
            //第一条语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,WRNF,STW ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD=1) ");
            strSql.Append("AND (tba.STCD=@STCD)");
            strSql.Append("AND (idtm >@sdate) AND (idtm<@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC; ");

            //第二条语句
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,WRNF,STW ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD=1) ");
            strSql.Append("AND (tba.STCD=@STCD)");
            strSql.Append("AND (idtm >@state_history) AND (idtm<@edate_history) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC; ");

            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(),sqlParams))
            {
                data.real = multi.Read<ST_SEDRFEntity>();
                data.history = multi.Read<ST_SEDRFEntity>();
            }

            return data;
        }

    }
}
