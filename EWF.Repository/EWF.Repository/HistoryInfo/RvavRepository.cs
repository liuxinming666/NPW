/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/25 16:46:14
 * 描  述：SedrfComparativeRepository
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
using EWF.Util.Page;

namespace EWF.Repository
{
    public class RvavRepository: DefaultRepository<ST_RVAVEntity>, IRvavRepository
    {
		private readonly string V_ST_RIVSAND_R;
		private readonly string ST_STBPRP_V;
		private readonly string PrimaryTableName;
		public RvavRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			PrimaryTableName = $"{RTDB_Schema}ST_RVAV_R";
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			V_ST_RIVSAND_R = $"{Default_Schema}V_ST_RIVSAND_R";
		}


		#region 水情均值-管理维护
		public Page<ST_RVAVEntity> GetPageData(int pageIndex, int pageSize, string STCD,string STTDRCD, string addvcd, string type)
        {
            var strSql = new StringBuilder();
            strSql.Append("(SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb  ");
			strSql.Append($"ON tba.stcd = tbb.stcd and tbb.TYPE='{type}' and tbb.ADDVCD='{addvcd}') tttt");
			
			var tableName = strSql.ToString();
            var fileds = "STNM,STCD,IDTM,STTDRCD,AVZ,AVQ";
            var where = " 1=1 ";
            var orderby = "STCD ASC,STTDRCD DESC,IDTM ASC";

            var sqlParams = new DynamicParameters();
			sqlParams.Add(nameof(type), type);
			sqlParams.Add(nameof(addvcd), addvcd);
			if (!STCD.IsEmpty())
            {
                where +=$"AND (STCD=@'{STCD}') ";
                //where +="AND (STCD=@STCD) ";
				sqlParams.Add(nameof(STCD), STCD);
            }
            if (!STTDRCD.IsEmpty())
            {
                where += $"AND (STTDRCD='{STTDRCD}') ";
                //where += "AND (STTDRCD=@STTDRCD) ";
				sqlParams.Add(nameof(STTDRCD), STTDRCD);
            }


			//var page = database.GetListPaged<ST_RVAVEntity>(pageIndex, pageSize, tableName, fileds, where, orderby, sqlParams);
			var page = database.GetPagination_Join<ST_RVAVEntity>(pageIndex, pageSize, tableName, fileds, where, orderby);
			return page;
        }
        #endregion

        #region 水情均值-统计-分析
        /// <summary>水情均值-统计-多日</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日均值</returns>
        /// <exception cref="ArgumentNullException">sdate - 开始日期
        /// or
        /// edate - 结束日期</exception>
        public IEnumerable<ST_RVAVEntity> GetDayData(string STCD, string addvcd, string type, string sdate, string edate)
        {
            #region 参数校验
            if (sdate.IsEmpty())
                throw new ArgumentNullException(nameof(sdate), "开始日期");
            if (edate.IsEmpty())
                throw new ArgumentNullException(nameof(edate), "结束日期");
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);

            var strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD=1) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<@edate) ");

            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_RVAVEntity>(strSql.ToString(), sqlParams);
        }

       
        /// <summary>水情均值-统计-旬月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内旬月均值</returns>
        /// <exception cref="ArgumentNullException">sdate - 开始日期
        /// or
        /// edate - 结束日期</exception>
        public IEnumerable<ST_RVAVEntity> GetMonthData(string STCD, string addvcd, string type, string sdate, string edate)
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
            var strSql = new StringBuilder();
			strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
			strSql.Append("WHERE (STTDRCD IN (4,5) ) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_RVAVEntity>(strSql.ToString(), sqlParams);
        }

        /// <summary>水情均值-统计-年</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>指定年份12个月的月均值</returns>
        /// <exception cref="ArgumentNullException">sdate - 开始日期
        /// or
        /// edate - 结束日期</exception>
        public IEnumerable<ST_RVAVEntity> GeYearData(string STCD,string addvcd,string type, string sdate, string edate)
        {
            #region 参数校验
            if (sdate.IsEmpty())
                throw new ArgumentNullException(nameof(sdate), "开始日期");
            if (edate.IsEmpty())
                throw new ArgumentNullException(nameof(edate), "结束日期");
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);
            var strSql = new StringBuilder();
			strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
			strSql.Append("WHERE (STTDRCD=5)");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD.Split(","));
                strSql.Append("AND (tba.STCD in @STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<=@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_RVAVEntity>(strSql.ToString(), sqlParams);
        }

        private IEnumerable<ST_RVAVEntity> GetData(string STCD, string addvcd, string type, string avgType, string sdate, string edate) {
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
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
			strSql.Append("WHERE (STTDRCD=1) ");
            if (!STCD.IsEmpty())
            {
                sqlParams.Add(nameof(STCD), STCD);
                strSql.Append("AND (tba.STCD=@STCD)");
            }
            strSql.Append("AND (idtm >@sdate) AND (idtm<@edate) ");

            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC ");

            return database.Connection.Query<ST_RVAVEntity>(strSql.ToString(), sqlParams);
        }


        /// <summary>
        /// 查询水情信息-未分页
        /// add by SUN
        /// Date:2019-05-23 17:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<ST_RIVEREntity> GetDayData_River(string stcd, string addvcd, string type, string startDate, string endDate)
        {
            //分组排序sql语句
            var sqlParams = new DynamicParameters();
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);

            var strSqlInnerText = $"select A.STCD,A.STNM,B.TM,B.Z,B.Q,B.FLWCHRCD,B.WPTN,B.MSQMT,B.XSA,B.S from (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) A,{V_ST_RIVSAND_R} B where A.STCD=B.STCD ";
            //参数列表
           
            //var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcd))
            {
                strSqlInnerText += " and A.STCD in @STCD ";
                sqlParams.Add("STCD", stcd.Split(","));
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and TM>@StartDate ";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and TM<=@EndDate ";
                sqlParams.Add("EndDate", endDate);
            }

            strSqlInnerText += " ORDER BY A.STCD ASC,TM ASC ";
            
            using (var db = database.Connection)
            {
                return db.Query<ST_RIVEREntity>(strSqlInnerText, sqlParams);
            }
        }

        

        /// <summary>
        /// 水情均值多日对比分析-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="avgType">均值类型</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="edate_history">对比结束日期</param>
        /// <returns>{real：分析数据，history：对比数据}</returns>
        /// <exception cref="ArgumentNullException">
        /// 测站编码
        /// or
        /// 开始日期
        /// or
        /// 结束日期
        /// or
        /// 对比年份-开始日期
        /// or
        /// 对比年份-开始日期
        /// </exception>
        public dynamic GetData_Comparative(string STCD, string addvcd, string type, string avgType, string sdate, string edate, string sdate_history, string edate_history)
        {
            #region 参数校验
            if (STCD.IsEmpty())
            {
                throw new ArgumentNullException("测站编码");
            }
            if (avgType.IsEmpty())
            {
                throw new ArgumentNullException("均值类型");
            }
            if (sdate.IsEmpty())
            {
                throw new ArgumentNullException("开始日期");
            }
            if (edate.IsEmpty())
            {
                throw new ArgumentNullException("结束日期");
            }
            if (sdate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            if (edate_history.IsEmpty())
            {
                throw new ArgumentNullException("对比年份-开始日期");
            }
            #endregion
            
            var sqlParams = new DynamicParameters();
            sqlParams.Add("STTDRCD", avgType);
            sqlParams.Add("STCD", STCD);
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);
            sqlParams.Add("state_history", sdate_history);
            sqlParams.Add("edate_history", edate_history);
            sqlParams.Add("ADDVCD", addvcd);
            sqlParams.Add("TYPE", type);
            //第一条语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
			strSql.Append("WHERE (STTDRCD = @STTDRCD) ");
            strSql.Append("AND (tba.STCD=@STCD)");
            strSql.Append("AND (idtm >@sdate) AND (idtm<@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC; ");

            //第二条语句
            strSql.Append("SELECT tbb.STNM,tba.STCD,IDTM,STTDRCD,AVZ,AVQ ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN (select * from {ST_STBPRP_V}  where ADDVCD=@ADDVCD and TYPE=@TYPE) tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (STTDRCD = @STTDRCD) ");
            strSql.AppendFormat("AND (tba.STCD =@STCD)");
            strSql.Append("AND (idtm >@state_history) AND (idtm<@edate_history) ");
            strSql.Append("ORDER BY tba.STCD ASC,IDTM ASC,STTDRCD DESC; ");

            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.real = multi.Read<ST_RVAVEntity>();
                data.history = multi.Read<ST_RVAVEntity>();
            }

            return data;
        }
        /// <summary>
        /// 水情均值多日对比分析-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="edate_history">对比结束日期</param>
        /// <returns>{real：分析数据，history：对比数据}</returns>
        /// <exception cref="ArgumentNullException">
        /// 测站编码
        /// or
        /// 开始日期
        /// or
        /// 结束日期
        /// or
        /// 对比年份-开始日期
        /// or
        /// 对比年份-开始日期
        /// </exception>
        public dynamic GetDayData_Comparative(string STCD, string addvcd, string type, string sdate, string edate, string sdate_history, string edate_history)
        {
            return GetData_Comparative(STCD,addvcd,type, "1", sdate, edate, sdate_history, edate_history);
        }

        #endregion
    }
}
