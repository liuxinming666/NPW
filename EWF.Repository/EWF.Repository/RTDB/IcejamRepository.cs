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
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace EWF.Repository
{
    public class IcejamRepository :DefaultRepository, IIcejamRepository
    {
        private IDatabase database;

		//private IDatabase database_oracle;
		//private IDatabase database_sql;

		private readonly string V_ST_TMP_RV;
		private readonly string ST_STBPRP_V;


		public IcejamRepository(IOptionsSnapshot<DbOption> options):base(options)
        {
			V_ST_TMP_RV = $"{Default_Schema}V_ST_TMP_RV";
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
		}

        /// <summary>
        /// 查询单站水温数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSingleTempData(string stcd, string startDate, string endDate)
        {
            //分组排序sql语句
            string strSqlInnerText = $" SELECT CONVERT(varchar(100), IDTM, 20) AS TM ,MXATMP AS HTMP,MNATMP AS LTMP,AVATMP AS AVTP,AVWTMP AS AVWT FROM {V_ST_TMP_RV} where ";
            string strSqlUnionText = $" union  SELECT  '平均' as TM ,round(avg(MXATMP),2) AS HTMP,round(avg(MNATMP),2) AS LTMP,round(avg(AVATMP),2) AS AVTP,round(avg(AVWTMP),2) AS AVWT FROM {V_ST_TMP_RV} WHERE ";
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
            string strSqlOuterText = strSqlInnerText+ strSqlUnionText;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }
        /// <summary>
        /// 查询多站水温数据-未分页
        /// add by Zhao
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTempDataByMultiStcds(string stcds, string startDate, string endDate)
        {
            var sqlText = $"SELECT a.stcd as STCD, b.stnm as STNM,  CONVERT(varchar(100), IDTM, 20) AS TM ,MXATMP AS HTMP,MNATMP AS LTMP,AVATMP AS AVTP,AVWTMP AS AVWT FROM {V_ST_TMP_RV} a,{RTDB_Schema}ST_STBPRP_B b  WHERE a.stcd=b.stcd   ";
            string strSqlUnionText = $" union SELECT a.stcd as STCD, b.stnm as STNM, '平均' as TM ,round(avg(MXATMP),2) AS HTMP,round(avg(MNATMP),2) AS LTMP,round(avg(AVATMP),2) AS AVTP,round(avg(AVWTMP),2) AS AVWT FROM{V_ST_TMP_RV} a,{RTDB_Schema}ST_STBPRP_B b  WHERE a.stcd=b.stcd  ";
            var sqlParams = new DynamicParameters();
            if (!string.IsNullOrEmpty(stcds))
            {
                sqlText += " and a.STCD in @STCDS ";
                strSqlUnionText += " and a.STCD in @STCDS";
                sqlParams.Add("STCDS", stcds.Split(','));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlText += " and IDTM>@StartDate";
                strSqlUnionText += " and IDTM>@StartDate";
                sqlParams.Add("StartDate", startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlText += " and IDTM<=@EndDate";
                strSqlUnionText += " and IDTM<=@EndDate";
                sqlParams.Add("EndDate", endDate);
            }

            strSqlUnionText += " group by a.STCD,stnm ORDER BY  TM";
            string strSqlOuterText = sqlText + strSqlUnionText;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSqlOuterText, sqlParams);
            }
        }
        /// <summary>
        /// 获取查询条件智能显示信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSearchKeywords(string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            string LikeCondition = "";
            if (string.IsNullOrEmpty(keyword))
            {
                LikeCondition = "";
            }
            else
            {
                LikeCondition = " where id= '" + keyword + "'";
            }
            strSql.Append("select  ")
                .Append(" id,name from Qelemet  ")
                 .Append(LikeCondition);

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString());
            }
        }

        /// <summary>
        /// 查询实时凌情
        /// </summary>
        /// add by lw
        /// <param name="stcds">站码列表，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区编码</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public DataTable GetIceData(string stcds, string startDate, string endDate, string addvcd, string type)
        {
            string Sqla = "";
            if (stcds != null && stcds != "")
            {
                Sqla = " AND S.STCD IN(" + stcds + ") ";
            }
			
            string Sqlb = ",(CASE WHEN F.QNTICD='1' THEN (CASE WHEN F.LBDIWD >0 THEN '左岸冰宽：' +cast(ltrim(Round(F.LBDIWD,0,1)) as varchar(20)) + '/10<BR>' END) + " +
                " (CASE WHEN F.RBDIWD>0 THEN '右岸冰宽：' + cast(ltrim(Round(F.RBDIWD,0,1)) as varchar(20)) +  '/10<BR>' END ) +  (CASE WHEN F.BDITHK>0 THEN '岸冰厚度：' + " +
                " substring(cast(ltrim(Round(F.BDITHK,2)) as varchar(20)),1,charIndex('.',cast(ltrim(Round(F.BDITHK,2))as varchar(20)))+2) + 'cm<BR>' END) " +
                            " WHEN F.QNTICD='3' OR  F.QNTICD='2' THEN (CASE WHEN F.IRCON >0 THEN '流冰密度：' + cast(ltrim(Round(F.IRCON,0,1)) as varchar(20)) +  '/10<BR>' END) + " +
                            " (CASE WHEN F.DITHK= 0 THEN '' WHEN F.DITHK >0 THEN '流冰厚度：' + cast(ltrim(Round(F.DITHK,2)) as varchar(20)) +  'cm<BR>' END ) +  (CASE WHEN F.MXIA>0 THEN '最大冰面积：'" +
                            " +cast(ltrim(Round(F.MXIA,2)) as varchar(20)) +  '平米<BR>' END) +  (CASE WHEN F.MXIV>0 THEN '最大冰流速：' + cast(LTRIM(Round(F.MXIV,2)) as varchar(20)) +  'm/s<BR>' END) + " +
                            " (CASE WHEN F.IQ>0 THEN '流冰量：' + cast(LTRIM(Round(F.IQ,2)) as varchar(20)) +  '立方m/s<BR>' END)" +
                            " WHEN F.QNTICD='4' THEN (CASE WHEN F.RLPSTN='0'  THEN '河段'  WHEN F.RLPSTN='1'  THEN '距河段上游'  WHEN F.RLPSTN='2'  THEN '距河段下游'    END) + " +
                            " (CASE WHEN F.RLDSTN>0  THEN '' + cast(ltrim(Round(F.RLDSTN,2)) as varchar(20)) +  'km处'  END) +  (CASE WHEN F.FRZPROP='1'  THEN '平封<BR>'  WHEN F.FRZPROP='2' THEN '立封<BR>'  END) " +
                            "WHEN F.QNTICD='5' THEN  (CASE WHEN F.BRKPROP='1' THEN '文开' WHEN F.BRKPROP='2' THEN '武开'  WHEN F.BRKPROP='3' THEN '半文半武开' END ) " +
                            "WHEN F.QNTICD='6' THEN (CASE WHEN F.DIPCK>0 THEN '流冰堆积：' + cast(ltrim(Round(F.DIPCK,0,1)) as varchar(20)) +  '/10' END)  " +
                            "WHEN F.QNTICD='7' THEN (CASE WHEN F.IDAMGRW ='0' THEN '冰坝发展：稳定<BR>' WHEN F.IDAMGRW ='1' THEN '冰坝发展：增强<BR>' WHEN F.IDAMGRW ='2' " +
                            "THEN '冰坝发展：减弱<BR>' END) + (CASE WHEN F.IDAMHGT> 0 THEN '冰坝高度：'  + cast(LTRIM(Round(F.IDAMHGT,2)) as varchar(20)) +  'm<BR>' END) + " +
                            " (CASE WHEN F.IDAMWD> 0 THEN '冰坝宽度：'  + cast(LTRIM(Round(F.IDAMWD,3)) as varchar(20)) +  'm<BR>' END) +  (CASE WHEN F.IDAMUPWPTN='4' " +
                            "THEN '上游水势：落<BR>'WHEN F.IDAMUPWPTN='5' THEN '上游水势：涨<BR>' WHEN F.IDAMUPWPTN='6' THEN '上游水势：平<BR>' END )  END) AS DLICE ";

            string Sqlc = "(CASE WHEN F.QNTICD='1' THEN '岸冰' WHEN F.QNTICD='2' THEN '流冰花' WHEN F.QNTICD='3' THEN '流冰' WHEN F.QNTICD='4' THEN '封冻' WHEN F.QNTICD='5'" +
                " THEN '解冻' WHEN F.QNTICD='6' THEN '流冰堆积' WHEN F.QNTICD='7' THEN '冰坝'   END) AS DLID " + Sqlb;

            //气温信息
            var sqlParamsaa = new DynamicParameters();
            //string Sqlaa = "SELECT B.RVNM,B.STNM,B.STCD,S.IDTM AS TM ,S.MXATMP AS HTMP,S.MNATMP AS LTMP,S.AVATMP AS AVTP,S.AVWTMP AS AVWT"
            //    + " FROM ST_STBPRP_B B,V_ST_TMP_RV S WHERE B.STCD=S.STCD AND  S.IDTM >@StartDate and S.IDTM<=@EndDate " + Sqla + " ORDER BY S.STCD,S.IDTM";
            string Sqlaa = "SELECT B.RVNM,B.STNM,B.STCD,S.IDTM AS TM ,S.MXATMP AS HTMP,S.MNATMP AS LTMP,S.AVATMP AS AVTP,S.AVWTMP AS AVWT"
              + " FROM "+ ST_STBPRP_V + " B,"+V_ST_TMP_RV+" S WHERE B.STCD=S.STCD AND B.ADDVCD=@ADDVCD AND B.TYPE=@TYPE AND  S.IDTM >@StartDate and S.IDTM<=@EndDate " + Sqla + " ORDER BY S.STCD,S.IDTM";
            SqlParameter[] parametersaa = {
                    new SqlParameter("@StartDate", SqlDbType.VarChar),
                     new SqlParameter("@EndDate", SqlDbType.VarChar),
                      new SqlParameter("@ADDVCD", SqlDbType.VarChar),
                       new SqlParameter("@TYPE", SqlDbType.VarChar)
            };
            parametersaa[0].Value = startDate;
            parametersaa[1].Value = endDate;
            parametersaa[2].Value = addvcd;
            parametersaa[3].Value = type;
            DataTable vtablea = database.FindTable(Sqlaa, parametersaa);

            //定性冰情
            var sqlParamsbb = new DynamicParameters();
            //string Sqlbb = "select b.rvnm,b.stnm,a.stcd,a.tm,a.dxid,a.dxice,a.IOSNDP,a.IUDFSTHK from (SELECT STCD , TM ,"
            // + "R.QLTICD AS DXID,( CASE WHEN R.QLTITHK  IS NOT NULL THEN '冰厚：'  +cast(ltrim(Round(R.QLTITHK,2)) as varchar(20)) +  'cm<BR>' ELSE '' END ) AS DXICE," +
            // "( CASE WHEN R.IOSNDP >0 THEN '冰上雪深：'  + cast(ltrim(Round(R.IOSNDP,2)) as varchar(20)) +  'cm<BR>' ELSE '' END ) AS IOSNDP," +
            // "( CASE WHEN R.IUDFSTHK>0 THEN '冰下冰花厚：' + cast(ltrim(Round(R.IUDFSTHK,0,1)) as varchar(20)) + '/10' ELSE '' END ) AS IUDFSTHK"
            // + " FROM ST_QLICEINF_R R WHERE  R.TM>@StartDate and R.TM<=@EndDate  " + Sqla.Replace("S.", "") + " ) a inner join ST_STBPRP_B B on" +
            // " a.stcd = b.stcd ORDER BY a.STCD,a.TM";
            string Sqlbb = "select b.rvnm,b.stnm,a.stcd,a.tm,a.dxid,a.dxice,a.IOSNDP,a.IUDFSTHK from (SELECT STCD , TM ,"
            + "R.QLTICD AS DXID,( CASE WHEN R.QLTITHK  IS NOT NULL THEN '冰厚：'  +cast(ltrim(Round(R.QLTITHK,2)) as varchar(20)) +  'cm<BR>' ELSE '' END ) AS DXICE," +
            "( CASE WHEN R.IOSNDP >0 THEN '冰上雪深：'  + cast(ltrim(Round(R.IOSNDP,2)) as varchar(20)) +  'cm<BR>' ELSE '' END ) AS IOSNDP," +
            "( CASE WHEN R.IUDFSTHK>0 THEN '冰下冰花厚：' + cast(ltrim(Round(R.IUDFSTHK,0,1)) as varchar(20)) + '/10' ELSE '' END ) AS IUDFSTHK"
            + " FROM "+RTDB_Schema+"ST_QLICEINF_R R WHERE  R.TM>@StartDate and R.TM<=@EndDate  " + Sqla.Replace("S.", "") + " ) a inner join "+ST_STBPRP_V+" B on" +
            " a.stcd = b.stcd  where B.ADDVCD=@ADDVCD AND B.TYPE=@TYPE ORDER BY a.STCD,a.TM";
            SqlParameter[] parametersbb = {
                    new SqlParameter("@StartDate", SqlDbType.VarChar),
                     new SqlParameter("@EndDate", SqlDbType.VarChar),
                     new SqlParameter("@ADDVCD", SqlDbType.VarChar),
                       new SqlParameter("@TYPE", SqlDbType.VarChar)
            };
            parametersbb[0].Value = startDate;
            parametersbb[1].Value = endDate;
            parametersbb[2].Value = addvcd;
            parametersbb[3].Value = type;

            DataTable vtableb = database.FindTable(Sqlbb, parametersbb);
            vtableb.TableName = "vtableb";

            //定量冰情
            //string Sqlcc = "select b.rvnm,b.stnm,a.stcd,a.tm,a.DLID,a.DLICE from (SELECT  STCD," +
            //    "  TM ," + Sqlc
            //    + " FROM ST_QTICEINF_R F WHERE   F.TM >@StartDate and F.TM<=@EndDate" + Sqla.Replace("S.", "") + " ) a inner join ST_STBPRP_B B" +
            //    " on a.stcd = b.stcd  ORDER BY a.STCD,a.TM";
            string Sqlcc = "select b.rvnm,b.stnm,a.stcd,a.tm,a.DLID,a.DLICE from (SELECT  STCD," +
               "  TM ," + Sqlc
               + " FROM "+RTDB_Schema+"ST_QTICEINF_R F WHERE   F.TM >@StartDate and F.TM<=@EndDate" + Sqla.Replace("S.", "") + " ) a inner join "+ST_STBPRP_V+" B" +
               " on a.stcd = b.stcd  where B.ADDVCD=@ADDVCD AND B.TYPE=@TYPE ORDER BY a.STCD,a.TM";
            SqlParameter[] parameterscc = {
                    new SqlParameter("@StartDate", SqlDbType.VarChar),
                     new SqlParameter("@EndDate", SqlDbType.VarChar),
                     new SqlParameter("@ADDVCD", SqlDbType.VarChar),
                       new SqlParameter("@TYPE", SqlDbType.VarChar)
            };
            parameterscc[0].Value = startDate;
            parameterscc[1].Value = endDate;
            parameterscc[2].Value = addvcd;
            parameterscc[3].Value = type;

            DataTable vtablec = database.FindTable(Sqlcc, parameterscc);
            vtablec.TableName = "vtablec";


            string sqlriver = "select stcd,idtm as tm,avq from " + RTDB_Schema + "st_rvav_r where idtm>@StartDate and idtm<=@EndDate and sttdrcd='1'" + Sqla.Replace("S.", "") + "";
            //参数列表
            SqlParameter[] parametersr = {
                    new SqlParameter("@StartDate", SqlDbType.VarChar),
                     new SqlParameter("@EndDate", SqlDbType.VarChar),
            };
            parametersr[0].Value = startDate;
            parametersr[1].Value = endDate;

            DataTable vtableR = database.FindTable(sqlriver, parametersr);
            vtableR.TableName = "vtabler";

            DataSet ds = new DataSet();
            ds.Tables.Add(vtablea);
            ds.Tables.Add(vtableb);
            ds.Tables.Add(vtablec);
            ds.Tables.Add(vtableR);

            System.Data.DataTable table = new System.Data.DataTable();
            ds.Tables[0].Columns.Add("AVQ");
            ds.Tables[0].Columns.Add("DXID");
            ds.Tables[0].Columns.Add("DXICE");
            ds.Tables[0].Columns.Add("DLID");
            ds.Tables[0].Columns.Add("DLICE");

            for (int i = 1; i < ds.Tables[2].Rows.Count; i++)
            {
                if (ds.Tables[2].Rows[i]["STCD"].ToString() == ds.Tables[2].Rows[i - 1]["STCD"].ToString() && ds.Tables[2].Rows[i]["TM"].ToString() == ds.Tables[2].Rows[i - 1]["TM"].ToString())
                {
                    ds.Tables[2].Rows[i - 1]["DLID"] = ds.Tables[2].Rows[i - 1]["DLID"].ToString() + "<BR>" + ds.Tables[2].Rows[i]["DLID"].ToString();
                    ds.Tables[2].Rows[i - 1]["DLICE"] = ds.Tables[2].Rows[i - 1]["DLICE"].ToString() + "<BR>" + ds.Tables[2].Rows[i]["DLICE"].ToString();
                    ds.Tables[2].Rows.RemoveAt(i);
                    i--;
                }
            }
            bool isEqual = false;
            System.Data.DataRow new_row = null;
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                isEqual = false;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    ds.Tables[0].Rows[j]["STCD"] = ds.Tables[0].Rows[j]["STCD"].ToString().Trim();
                    if (ds.Tables[1].Rows[i]["STCD"].ToString().Trim() == ds.Tables[0].Rows[j]["STCD"].ToString().Trim() && ds.Tables[1].Rows[i]["TM"].ToString() == ds.Tables[0].Rows[j]["TM"].ToString())
                    {
                        ds.Tables[0].Rows[j]["DXID"] = ds.Tables[1].Rows[i]["DXID"].ToString().Trim();
                        ds.Tables[0].Rows[j]["DXICE"] = ds.Tables[1].Rows[i]["DXICE"].ToString().Trim();
                        isEqual = true;
                    }
                }
                if (!isEqual)
                {
                    new_row = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(new_row);
                    new_row["STCD"] = ds.Tables[1].Rows[i]["STCD"].ToString().Trim();
                    new_row["TM"] = ds.Tables[1].Rows[i]["TM"].ToString().Trim();
                    new_row["DXID"] = ds.Tables[1].Rows[i]["DXID"].ToString().Trim();
                    new_row["DXICE"] = ds.Tables[1].Rows[i]["DXICE"].ToString().Trim();

                    new_row["STNM"] = ds.Tables[1].Rows[i]["STNM"].ToString();
                    new_row["RVNM"] = ds.Tables[1].Rows[i]["RVNM"].ToString().Trim();
                }
            }

            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {
                isEqual = false;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (ds.Tables[2].Rows[i]["STCD"].ToString().Trim() == ds.Tables[0].Rows[j]["STCD"].ToString().Trim() && ds.Tables[2].Rows[i]["TM"].ToString() == ds.Tables[0].Rows[j]["TM"].ToString())
                    {
                        ds.Tables[0].Rows[j]["DLID"] += ds.Tables[2].Rows[i]["DLID"].ToString().Trim() + "<BR>";
                        ds.Tables[0].Rows[j]["DLICE"] += ds.Tables[2].Rows[i]["DLICE"].ToString().Trim();
                        isEqual = true;
                    }
                }
                if (!isEqual)
                {
                    new_row = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(new_row);
                    new_row["STCD"] = ds.Tables[2].Rows[i]["STCD"].ToString();
                    new_row["TM"] = ds.Tables[2].Rows[i]["TM"].ToString();
                    new_row["DLID"] += ds.Tables[2].Rows[i]["DLID"].ToString().Trim() + "<BR>";
                    new_row["DLICE"] += ds.Tables[2].Rows[i]["DLICE"].ToString().Trim();
                    new_row["STNM"] = ds.Tables[2].Rows[i]["STNM"].ToString();
                    new_row["RVNM"] = ds.Tables[2].Rows[i]["RVNM"].ToString().Trim();
                }
            }

            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (ds.Tables[3].Rows[i]["STCD"].ToString().Trim() == ds.Tables[0].Rows[j]["STCD"].ToString().Trim() && ds.Tables[3].Rows[i]["TM"].ToString() == ds.Tables[0].Rows[j]["TM"].ToString())
                    {
                        ds.Tables[0].Rows[j]["AVQ"] = ds.Tables[3].Rows[i]["AVQ"].ToString().Trim();

                    }
                }

            }

            return ds.Tables[0];

        }

        /// <summary>
        /// 获取凌情动态信息sql 版本
        /// </summary>
        /// add by lw
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable GetIceJamDynamic_sql(string startDate, string endDate)
        {

            string sql = $"SELECT * from(select rank() over(partition by A.STCDB,A.STCDE,A.SORTNO order by YMDHM desc) r,A.* FROM {RTDB_Schema}TBL_LQDT A WHERE" +
                " YMDHM>=@StartDate and YMDHM<= @EndDate) where r=1";
            //参数列表
            SqlParameter[] parametersr = {
                    new SqlParameter("@StartDate", SqlDbType.VarChar),
                     new SqlParameter("@EndDate", SqlDbType.VarChar),
            };
            parametersr[0].Value = startDate;
            parametersr[1].Value = endDate;

            DataTable vtable = database.FindTable(sql, parametersr);
            return vtable;
        }

		/// <summary>
		/// 获取凌情动态信息oracle 版本
		/// </summary>
		/// <remarks>
		/// 此方法不可用：Oracle版本单独建立在Repository
		/// </remarks>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public DataTable GetIceJamDynamic(string Sdate)
        {
            string StartDate = Convert.ToDateTime(Sdate).AddDays(-3).ToString("yyyy-MM-dd 08:00");
            string endDate = Convert.ToDateTime(Sdate).ToString("yyyy-MM-dd 08:00");
        
            string strSQL = "SELECT * from(select rank() over(partition by A.STCDB,A.STCDE,A.SORTNO order by YMDHM desc) r,A.* FROM TBL_LQDT A WHERE " +
                "YMDHM>= to_date('"+ StartDate + "','RRRR-MM-DD HH24:MI') and " +
                "YMDHM<= to_date('"+ endDate + "','RRRR-MM-DD HH24:MI')) where r=1";
            //参数列表
            //OracleParameter[] parametersr = {
            //        new OracleParameter("@startDate", OracleDbType.NVarchar2),
            //         new OracleParameter("@endDate", OracleDbType.NVarchar2),
            //};
            //parametersr[0].Value = startDate;
            //parametersr[1].Value = endDate;
            DataTable vtable = database.FindTable(strSQL);
            return vtable;
        }

        /// <summary>
        /// 获取凌情中关注的站点水情信息
		/// TODO：此方法有问题，在实时库中有视图
        /// </summary>
        /// <param name="Sdate"></param>
        /// <returns></returns>
        public DataTable GetIceJam_River(string Sdate)
        {
            DataSet ds = new DataSet();
            string startDate = Convert.ToDateTime(Sdate).AddDays(-3).ToString();
            string endDate = Sdate;
            string StDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd 08:00:00");
            string etDate= Convert.ToDateTime(endDate).ToString("yyyy-MM-dd 08:00:00");
            string strSQL = " SELECT* FROM  "
            + " (SELECT rank() over(partition by A.STCD order by A.TM  desc) r, A.STCD,rtrim(B.STNM) as STNM,B.LTTD,B.LGTD,A.Z,A.Q,C.QLTITHK,A.TM  "
            + " FROM "+Default_Schema + "V_ST_RIVSAND_R A inner join " + RTDB_Schema + "ST_STBPRP_B B on A.STCD=B.STCD left join " + RTDB_Schema + "ST_QLICEINF_R C on a.stcd = C.STCD and  a.tm = C.TM "
			+ " where A.TM>=@StartDate AND A.TM<=@EndDate and  A.STCD IN (SELECT STCD FROM " + Default_Schema + "TBL_REPORTSTATION WHERE REPORTTYPE='L3') ) t where r=1 ";
            SqlParameter[] parametersr = {
                    new SqlParameter("@StartDate", SqlDbType.VarChar),
                     new SqlParameter("@EndDate", SqlDbType.VarChar),
            };
            parametersr[0].Value = startDate;
            parametersr[1].Value = endDate;

            DataTable vtable = database.FindTable(strSQL, parametersr);
            return vtable;

        }

		/// <summary>
		/// TODO：此方法不可用，oracle单独放在repository中
		/// </summary>
		/// <param name="Sdate"></param>
		/// <returns></returns>
		public DataTable GetIceJam_LQDT(string Sdate)
        {
            string startDate = Convert.ToDateTime(Sdate).AddDays(-3).ToString();
            string endDate = Sdate;
            string StDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd 08:00:00");
            string etDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd 08:00:00");
            string strSQLbc = "select * from(SELECT CONTEXTSTR FROM TBL_REPORTTEXT WHERE REPORTTYPE='L2' AND REPORTFIELDNAME='凌情动态封河情况' "
             + " AND REPORTDATE>= to_date('" + StDate + "','RRRR-MM-DD HH24:MI:SS')" +
             " AND REPORTDATE<= to_date('" + etDate + "','RRRR-MM-DD HH24:MI:SS') order by REPORTDATE desc) where rownum=1";
            //OracleParameter[] parametersrbc = {
            //        new OracleParameter(":StartDate", OracleDbType.NVarchar2),
            //         new OracleParameter(":EndDate", OracleDbType.NVarchar2),
            //};
            //parametersr[0].Value = StDate;
            //parametersr[1].Value = etDate;
          
            DataTable vtablebc = database.FindTable(strSQLbc);
            return vtablebc;
        }
    }
}