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
    public class TmpavRepository: DefaultRepository,ITmpavRepository
	{
		private readonly string PrimaryTableName;
		public TmpavRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			PrimaryTableName = $"{Default_Schema}V_ST_TMP_RV";
		}
		/// <summary>
		/// 查询水温气温数据
		/// </summary>
		/// <param name="stcd"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public IEnumerable<dynamic> GetTempComparativeData(string stcd, string startDate, string endDate)
        {
            //分组排序sql语句
            string strSqlInnerText = $" SELECT CONVERT(varchar(100), IDTM, 20) AS TM ,MXATMP AS HTMP,MNATMP AS LTMP,AVATMP AS AVTP,AVWTMP AS AVWT,MXWTMP AS HWMP,MNWTMP AS LWMP FROM {PrimaryTableName} where ";
            string strSqlUnionText = $" union  SELECT  '平均' as TM ,round(avg(MXATMP),2) AS HTMP,round(avg(MNATMP),2) AS LTMP,round(avg(AVATMP),2) AS AVTP,round(avg(AVWTMP),2) AS AVWT,round(avg(MXWTMP),2) AS HWMP,round(avg(MNWTMP),2) AS LWMP FROM {PrimaryTableName} WHERE ";
            //参数列表
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcd))
            {
                strSqlInnerText += "  STCD=@STCD ";
                strSqlUnionText += " STCD=@STCD ";
                sqlParams.Add("STCD", stcd);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSqlInnerText += " and IDTM >@StartDate ";
                strSqlUnionText += " and IDTM >@StartDate ";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSqlInnerText += " and IDTM<=@EndDate";
                strSqlUnionText += " and IDTM<=@EndDate ";
                sqlParams.Add("EndDate", endDate);
            }
            strSqlUnionText += " group by STCD ORDER BY TM ";
            //获取每个分组中序号为1的数据，即最新一条数据
            string strSqlOuterText = strSqlInnerText + strSqlUnionText;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }
       
		#region 历史同期水温气温对比分析
        /// <summary>历史同期水温气温对比分析</summary>
        /// <param name="STCD">站名</param>
        /// <param name="STTDRCD">类型1表示日，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="end_history">对比结束日期</param>
        /// <returns>时段内旬月均值</returns>
        public dynamic GetData_MMonthEV(string STCD, string type, string sdate, string edate, string sdate_history, string edate_history)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("STCD", STCD);
            sqlParams.Add("STTDRCD", type);
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);
            sqlParams.Add("sdate_history", sdate_history);
            sqlParams.Add("edate_history", edate_history);
            StringBuilder strSql = new StringBuilder();
            //第一条语句
            strSql.Append("SELECT CONVERT(varchar(100), IDTM, 20) AS IDTM  ");
            // strSql.Append("SELECT CONVERT(varchar(100), IDTM, 20) AS IDTM ,MXATMP AS HTMP,MNATMP AS LTMP,AVATMP AS ACCP,AVWTMP AS AVWT,MXWTMP AS HWMP,MNWTMP AS LWMP FROM V_ST_TMP_RV tba where ");
            if (type == "1")
            {
                strSql.Append(" ,MXATMP AS ACCP ");
            }
            else if(type=="2")
            { 
                strSql.Append(" ,MNATMP AS ACCP ");
            }else if(type=="3")
            {
                strSql.Append(" ,MXWTMP AS ACCP ");
            }else if(type=="4")
            {
                strSql.Append(" ,MNWTMP AS ACCP ");
            }else if(type=="5")
            {
                strSql.Append(" ,AVATMP AS ACCP ");
            }else
            {
                strSql.Append(" ,AVWTMP AS ACCP ");
            }


            strSql.Append($"  FROM {PrimaryTableName} tba where STTDRCD='1' ");
            strSql.Append(" AND (tba.STCD=@STCD)");
            strSql.Append(" AND (idtm >@sdate) AND (idtm<=@edate) ");
			//strSql.Append(" union  SELECT  '平均' as IDTM ,round(avg(MXATMP),2) AS HTMP,round(avg(MNATMP),2) AS LTMP,round(avg(AVATMP),2) AS ACCP,round(avg(AVWTMP),2) AS AVWT,round(avg(MXWTMP),2) AS HWMP,round(avg(MNWTMP),2) AS LWMP FROM V_ST_TMP_RV tba WHERE ");
			//strSql.Append(" (STTDRCD=@STTDRCD)");
			//strSql.Append(" AND (tba.STCD=@STCD)");
			//strSql.Append(" AND (idtm >@sdate) AND (idtm<=@edate) ");
			//strSql.Append(" group by STCD ORDER BY IDTM ");
			//第二条语句
			strSql.AppendLine("SELECT CONVERT(varchar(100), IDTM, 20) AS IDTM ");

            if (type == "1")
            {
                strSql.Append(" ,MXATMP AS ACCP ");
            }
            else if (type == "2")
            {
                strSql.Append(" ,MNATMP AS ACCP ");
            }
            else if (type == "3")
            {
                strSql.Append(" ,MXWTMP AS ACCP ");
            }
            else if (type == "4")
            {
                strSql.Append(" ,MNWTMP AS ACCP ");
            }
            else if (type == "5")
            {
                strSql.Append(" ,AVATMP AS ACCP ");
            }
            else
            {
                strSql.Append(" ,AVWTMP AS ACCP ");
            }
			strSql.Append($"  FROM {PrimaryTableName} tba where STTDRCD='1' ");
            strSql.Append(" AND (tba.STCD=@STCD)");
            strSql.Append(" AND (idtm >@sdate_history) AND (idtm<=@edate_history) ");
            //strSql.Append(" union  SELECT  '平均' as IDTM ,round(avg(MXATMP),2) AS HTMP,round(avg(MNATMP),2) AS LTMP,round(avg(AVATMP),2) AS ACCP,round(avg(AVWTMP),2) AS AVWT,round(avg(MXWTMP),2) AS HWMP,round(avg(MNWTMP),2) AS LWMP FROM V_ST_TMP_RV tba WHERE ");
            //strSql.Append(" (STTDRCD=@STTDRCD)");
            //strSql.Append(" AND (tba.STCD=@STCD)");
            //strSql.Append(" AND (idtm >@sdate_history) AND (idtm<=@edate_history) ");
            //strSql.Append(" group by STCD ORDER BY IDTM ");
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
