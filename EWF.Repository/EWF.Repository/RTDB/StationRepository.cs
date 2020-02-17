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
using System.Dynamic;
using EWF.Util.Data;

namespace EWF.Repository
{
    public partial class StationRepository:DefaultRepository, IStationRepository
    {
        //private IDatabase database;
        //private IDatabase fileDataBase;

		private readonly string ST_STBPRP_V;
		public string File_DBName = EWFConsts.File_DBName;

		public StationRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
        }

        public IEnumerable<dynamic> GetStationList(string stcds, string addvcd, string type)
        {
            string sql = "SELECT * FROM "+ ST_STBPRP_V + " WHERE ADDVCD = '" + addvcd.Trim() + "' and TYPE = '" + type.Trim() + "' ";

            if (!stcds.IsNullOrEmpty())
            {
                sql += " and (STCD IN (" + stcds + "))";
            }

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sql);
            }
        }

        /// <summary>
        /// 获取查询条件智能显示信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="strType"></param>
        /// <param name="sttp">0:所有站点 1:雨情相关站  2：水情站点 3：水库站点 4:凌情站点</param>
        /// <param name="count"></param>
        /// <param name="type">1：行政区 2：流域</param>
        /// <param name="addvcd">行政区或者流域编码</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSearchKeywords(string keyword, string strType, int count,string sttp,int type, string addvcd)
        {
            StringBuilder strSql = new StringBuilder();
            string LikeCondition = "";
            var topFields = " top " + count.ToString();
            //显示行数小于等于0时，全部显示
            if (count <= 0)
            {
                topFields = "";
            }
            if (string.IsNullOrEmpty(keyword))
            {
                LikeCondition = "";
            }
            else
            {
                if (strType == "1") //拼音码
                {
                    LikeCondition = " and a.PHCD is not null and a.PHCD like '" + keyword + "%'";
                }
                //strSql.Append("select distinct top " + count.ToString() + " a.PHCD as KEYWORD, RTRIM(b.STNM) as KEYNAME, b.STCD as CODE, a.PHCD as PYM, b.STTP as TYPE from TBL_REGIONSET a,RWDB.dbo.ST_STBPRP_B b where a.STCD = b.STCD and a.PHCD is not null and a.PHCD like '" + keyword.ToUpper() + "%'");
                else if (strType == "2") //测站编码
                    LikeCondition = " and a.STCD is not null and a.STCD like '" + keyword.ToUpper() + "%'";
                    //strSql.Append("select distinct top " + count.ToString() + " a.STCD as KEYWORD, RTRIM(b.STNM) as KEYNAME, b.STCD as CODE, a.PHCD as PYM, b.STTP as TYPE from TBL_REGIONSET a,RWDB.dbo.ST_STBPRP_B b where a.STCD = b.STCD and a.STCD is not null and a.STCD like '" + keyword.ToUpper() + "%'");
                else
                    LikeCondition = " and a.STNM is not null and a.STNM like '" + keyword.ToUpper() + "%'";
            }
            string sttpwhere = " and a.TYPE=" + type + " and a.ADDVCD = '"+addvcd+"'";
            if(!string.IsNullOrWhiteSpace(sttp))
                sttpwhere += " and a.sttp in ("+sttp+")";

            strSql.Append("select distinct ")
                .Append(topFields)
                .Append(" a.PHCD as KEYWORD, RTRIM(a.STNM) as KEYNAME, a.STCD as CODE, a.STTP as TYPE,a.LGTD,a.LTTD from "+ ST_STBPRP_V + " a where 1=1 " + sttpwhere)
                .Append(LikeCondition);
            //strSql.Append("select distinct ")
            //    .Append(topFields)
            //    .Append(" a.PHCD as KEYWORD, RTRIM(b.STNM) as KEYNAME, b.STCD as CODE, a.PHCD as PYM, b.STTP as TYPE,b.LGTD,b.LTTD from TBL_REGIONSET a,EW_NPW.dbo.ST_STBPRP_V b where a.STCD = b.STCD " + sttpwhere)
            //    .Append(LikeCondition);
            /*if (strType == "1") //拼音码 
                  strSql.Append("select distinct top " + count.ToString() + " a.PHCD as KEYWORD, RTRIM(b.STNM) as KEYNAME, b.STCD as CODE, a.PHCD as PYM, b.STTP as TYPE from TBL_REGIONSET a,RWDB.dbo.ST_STBPRP_B b where a.STCD = b.STCD and a.PHCD is not null and a.PHCD like '" + keyword.ToUpper() + "%'");
              else if (strType == "2") //测站编码
                strSql.Append("select distinct top " + count.ToString() + " a.STCD as KEYWORD, RTRIM(b.STNM) as KEYNAME, b.STCD as CODE, a.PHCD as PYM, b.STTP as TYPE from TBL_REGIONSET a,RWDB.dbo.ST_STBPRP_B b where a.STCD = b.STCD and a.STCD is not null and a.STCD like '" + keyword.ToUpper() + "%'");
              else
                strSql.Append("select distinct top " + count.ToString() + " a.STNM as KEYWORD, RTRIM(b.STNM) as KEYNAME, b.STCD as CODE, a.PHCD as PYM, b.STTP as TYPE from TBL_REGIONSET a,RWDB.dbo.ST_STBPRP_B b where a.STCD = b.STCD and a.STNM is not null and a.STNM like '" + keyword.ToUpper() + "%'");
                */
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString());
            }
        }

        /// <summary>
        /// 查询测站要展示的三关系线数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetThreeLineData(string stcd, string sDate, string eDate)
        {
            StringBuilder strWhere = new StringBuilder();

            strWhere.Append(" a.stcd='" + stcd + "' and (a.MSQMT = '2' or a.MSQMT = '3' or a.MSQMT = '5')");
            strWhere.Append(" and a.TM >= '" + sDate + "' and");
            strWhere.Append(" a.TM <= '" + eDate + "'");
            strWhere.Append(" and a.STCD = b.STCD");
            strWhere.Append(" order by a.Z");

			string sql = @"select a.*,b.STNM from " + RTDB_Schema + "st_river_r a," + RTDB_Schema + "st_stbprp_b b where " + strWhere;

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sql);
            }
        }

        /// <summary>
        /// 过流断面数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="stnm"></param>
        /// <param name="year"></param>
        /// <param name="sDt"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetGLDMInfo(string stcd, string stnm, string year, string sDt)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            sqlParams.Add("stnm", stnm);
            sqlParams.Add("year", year.Split(','));
            sqlParams.Add("sDt", sDt.Split(','));

            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select SCSJ,GC,QDJ from {File_Schema}TBL_SCDM_SWZ where GC >=0 and ");
            strSql.Append("STCD = @stcd and rtrim(MS) = @stnm and ");
            strSql.Append("(DateName(YEAR,SCSJ)) in @year and DT in @sDt ");
            strSql.Append("order by SCSJ,QDJ asc");

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString(), sqlParams);
            }
        }

        /// <summary>
        /// 获取测站断面时间序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStationSectionYears(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select distinct (DateName(YEAR,SCSJ)) as TM,RTRIM(b.STNM) as STNM from {File_Schema}TBL_SCDM_SWZ a,{ST_STBPRP_V} b where a.STCD = @stcd and a.STCD = b.STCD;");

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString(), sqlParams);
            }
        }

        /// <summary>
        /// 获取雨量站信息数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRainStationInfo(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select * from {ST_STBPRP_V} where STCD = @stcd;");

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString(), sqlParams);
            }
        }

        /// <summary>
        /// 根据站码、年份和月份获取水位流量关系数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetZQRLYearMonthData(string stcd, string year)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            sqlParams.Add("year", year);

            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select STCD,BGTM,LNNM,PTNO,Z,Q,ZMEA,QMEA from {File_Schema}TBL_ZQRL ");
            strSql.Append("where STCD = @stcd and BGTM = @year ");
            strSql.Append("order by BGTM,PTNO");

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString(), sqlParams);
            }
        }

        /// <summary>
        /// 获取测站水位流量关系曲线有数据的年份和月份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStationZQRLYearMonth(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct rtrim(BGTM) as LNNM,(DateName(YEAR,BGTM) + '-' + DateName(MONTH,BGTM)) as LNNMTEXT ");
            strSql.Append($"from {File_Schema}tbl_zqrl ");
            strSql.Append("where stcd= @stcd ");
            strSql.Append("order by LNNM asc;");

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString(), sqlParams);
            }
        }

        /// <summary>
        /// 获取测站断面名称序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStationSectionNames(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select distinct a.MS,RTRIM(b.STNM) as STNM from {File_Schema}TBL_SCDM_SWZ a,{ST_STBPRP_V} b where a.STCD = @stcd and a.STCD = b.STCD;");

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(strSql.ToString(),sqlParams);
            }
        }

        /// <summary>
        /// 获取测站的断面信息，时间序列和断面名称序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public dynamic GetStationSectionInfo(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            //第一条语句,获取时间序列
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select distinct (DateName(YEAR,SCSJ)) as TM,RTRIM(b.STNM) as STNM from {File_Schema}TBL_SCDM_SWZ a,{ST_STBPRP_V} b where a.STCD = @stcd and a.STCD = b.STCD;");

            //第二条sql语句
            strSql.Append($"select distinct a.MS,RTRIM(b.STNM) as STNM from {File_Schema}TBL_SCDM_SWZ a,{ST_STBPRP_V} b where a.STCD = @stcd and a.STCD = b.STCD;");

            dynamic data = new ExpandoObject();

            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.list1 = multi.Read<dynamic>().AsList();
                data.list2 = multi.Read<dynamic>().AsList();
                data.list3 = multi.Read<dynamic>().AsList();
            }

            return data;
        }

        /// <summary>
        /// 获取水流沙过程线FLASH数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public dynamic GetCalLineData(string stcd,string sDate,string eDate)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            sqlParams.Add("sDate", sDate);
            sqlParams.Add("eDate", eDate);

            //第一条语句,根据站码获取站名
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT STNM FROM ST_STBPRP_B WHERE STCD=@stcd;");

            //获取水位流量过程线数据
            strSql.Append("SELECT DISTINCT A.RVNM,A.STCD,A.STNM,B.TM,");
            strSql.Append(" ( CASE WHEN B.FLWCHRCD='0' THEN '干枯' WHEN B.FLWCHRCD='@' THEN '流向不定' WHEN B.FLWCHRCD='<' THEN '逆流' WHEN B.FLWCHRCD='^' THEN '起涨' WHEN B.FLWCHRCD='*' THEN '<FONT  COLOR=RED>洪峰</FONT>' ELSE ' ' END ) AS 'FLWCHRCD', B.Z, ");
            strSql.Append(" ( CASE WHEN B.WPTN='4' THEN '<font color=green>落</font>' WHEN B.WPTN='5' THEN '<font color=red>涨</font>' WHEN B.WPTN='6' THEN '平' ELSE ' ' END) AS 'WPTN',B.Q,");
            strSql.Append(" (CASE WHEN B.MSQMT='2' THEN '浮标' WHEN B.MSQMT='3' THEN '流速仪' ELSE '' END ) AS 'MSQMT',");
            strSql.Append(" B.XSA,C.AVQ,B.S ");
            strSql.Append($" FROM {File_Schema}TBL_REGIONSTATION A , {Default_Schema}[V_ST_RIVSAND_R] B ,{RTDB_Schema}ST_RVAV_R C  ");
            strSql.Append(" WHERE ( A.REGIONTYPE='W' AND A.STCD = @stcd AND ((B.YMDHM)>=@sDate ");
            strSql.Append(" AND (B.YMDHM)<=@eDate)) ");
            strSql.Append(" AND A.STCD = B.STCD  AND C.STCD = B.STCD AND C.IDTM = B.YMDHM AND C.STTDRCD='1' ");
            strSql.Append(" ORDER BY A.STCD,B.YMDHM ASC;");

            //获取河道站防洪指标
            strSql.Append($"SELECT * FROM {RTDB_Schema}ST_RVFCCH_B WHERE STCD = @stcd");


            dynamic data = new ExpandoObject();

            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.list1 = multi.Read<dynamic>().AsList();
                data.list2 = multi.Read<dynamic>().AsList();
                data.list3 = multi.Read<dynamic>().AsList();
            }

            return data;
        }

    }
}
