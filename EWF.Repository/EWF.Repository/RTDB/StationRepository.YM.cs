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

namespace EWF.Repository
{
    public partial class StationRepository : IStationRepository
    {
        #region Add by YM

        #region 历年特征值
        ///add by ym
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd">站码</param>

        //public IEnumerable<dynamic> GetYearsZ(string stcd)
        //{
        //    StringBuilder strWhere = new StringBuilder();

        //    strWhere.Append(" where a.STCD = '" + stcd + "' and a.STTDRCD = '6' and a.STCD = b.STCD order by a.IDTM");

        //    string sql = @"select a.STCD,datename(year, a.IDTM) as 年份,a.MAXZ as 水位,a.MINZ,a.AVZ,a.STTDRCD,b.STNM from ST_ZSTAT_R a, RWDB.dbo.ST_STBPRP_B b " + strWhere;

        //    using (var db = fileDataBase.Connection)
        //    {
        //        return db.Query<dynamic>(sql);
        //    }
        //}
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd">站码</param>
        public IEnumerable<dynamic> GetYearsZ(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            string sqlInnerText = $"select a.STCD,datename(year, a.IDTM) as 年份,a.MAXZ as 水位,a.MINZ,a.AVZ,a.STTDRCD,b.STNM from {File_Schema}ST_ZSTAT_R a, {ST_STBPRP_V} b where a.STCD=@stcd and a.STTDRCD = '6' and a.STCD = b.STCD order by a.IDTM";

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 根据站码获取测站的年流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetYearsQ(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            string sqlInnerText = $"select STCD,datename(year, IDTM) as 年份,MAXQ as 流量,MINQ,AVQ,STTDRCD from {File_Schema}ST_QSTAT_R where STCD=@stcd and STTDRCD = '6'";

            sqlInnerText += " ORDER BY 年份"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 根据站码获取测站的年含沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetYearsSand(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            string sqlInnerText = $"select STCD,datename(year, IDTM) as 年份,MAXS as 含沙量,MINS,STTDRCD from {File_Schema}ST_SSTAT_R where STCD=@stcd and STTDRCD = '6'";

            sqlInnerText += " ORDER BY 年份"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 根据站码获取测站的年径流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetYearsJL(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            string sqlInnerText = $"select @stcd as STCD,'Av' as 年份,round(avg(ACCJL),2) as 径流量,null as RUMO,null as RUDE,'6' as STTDRCD from {File_Schema}ST_JLSTAT_R  where STCD=@stcd and STTDRCD = '6' union all select STCD,datename(year,IDTM) as 年份,ACCJL as 径流量,RUMO,RUDE,STTDRCD from {File_Schema}ST_JLSTAT_R where STCD=@stcd and STTDRCD = '6'";
            sqlInnerText += " ORDER BY 年份"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 根据站码获取测站的年输沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetYearsSSL(string stcd)
        {

            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            string sqlInnerText = $"select @stcd as STCD,'Av' as 年份,round(avg(STW),2) as 输沙量,null as STMO,'6' as STTDRCD from {File_Schema}ST_SEDRF_R where  STCD=@stcd and STTDRCD = '6' union all select STCD,datename(year,IDTM) as 年份,STW as 输沙量,STMO,STTDRCD from {File_Schema}ST_SEDRF_R where STCD=@stcd and STTDRCD = '6'";
            sqlInnerText += " ORDER BY 年份"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }

        }
        /// <summary>
        /// 根据站码获取测站的年降水量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetYearsRain(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            string sqlInnerText = $"select @stcd as STCD,'Av' as 年份,round(avg(ACCP),1) as 降水量,'6' as STTDRCD from {File_Schema}ST_PPSTAT_R where  STCD=@stcd and STTDRCD = '6' union all select STCD,datename(year,IDTM) as 年份,ACCP as 降水量,STTDRCD from {File_Schema}ST_PPSTAT_R where STCD =@stcd  and STTDRCD = '6'";
            sqlInnerText += " ORDER BY 年份"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        ///// <summary>
        ///// 根据条件获取测站的输沙量径流量
        ///// </summary>
        ///// <param name="stcd"></param>
        ///// <returns></returns>
        //public dynamic GetYearsSSLJLL(string stcd)
        //{
        //    var sqlParams = new DynamicParameters();
        //    sqlParams.Add("stcd", stcd);

        //    //第一条语句
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select @stcd as STCD,'Av' as 年份,round(avg(STW),2) as 输沙量,null as STMO,'6' as STTDRCD ");
        //    strSql.Append("FROM {File_Schema}ST_SEDRF_R tba JOIN {File_Schema}ST_STBPRP_B tbb ON tba.stcd = tbb.stcd ");
        //    strSql.Append($" from {File_Schema}ST_SEDRF_R where  STCD = @stcd and STTDRCD = '6' union all select STCD,datename(year,IDTM) as 年份,STW as 输沙量,STMO,STTDRCD from {File_Schema}ST_SEDRF_R ");
        //    strSql.Append(" where STCD = @stcd and STTDRCD = '6'");
        //    strSql.Append(" order by 年份 ");

        //    //第二条语句
        //    strSql.Append("select @stcd as STCD,'Av' as 年份,round(avg(ACCJL),2) as 径流量,null as RUMO,null as RUDE,'6' as STTDRCD ");
        //    strSql.Append($" FROM {File_Schema}ST_SEDRF_R tba JOIN {File_Schema}ST_STBPRP_B tbb ON tba.stcd = tbb.stcd ");
        //    strSql.Append(" from {File_Schema}ST_JLSTAT_R where STCD = @stcd and STTDRCD = '6' union all select STCD,datename(year,IDTM) as 年份,ACCJL as 径流量,RUMO,RUDE,STTDRCD from {File_Schema}ST_JLSTAT_R");
        //    strSql.Append(" where STCD = @stcd and STTDRCD = '6'");
        //    strSql.Append(" order by 年份 ");

        //    dynamic data = new ExpandoObject();
        //    using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
        //    {
        //        data.datasource = multi.Read<dynamic>().AsList();
        //    }

        //    return data;
        //}
        /// <summary>
        /// 根据条件获取测站的所有特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public dynamic GetAllCVData(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            //第一条语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select datename(year,IDTM) as IDTM,MAXZ,MINZ,AVZ from {File_Schema}ST_ZSTAT_R ");
            strSql.Append(" where STCD = @stcd");
            strSql.Append(" order by IDTM desc ");
            //第二条语句
            strSql.Append($"select datename(year,IDTM) as IDTM,MAXQ,MINQ,AVQ from {File_Schema}ST_QSTAT_R ");
            strSql.Append(" where STCD = @stcd");
            strSql.Append(" order by IDTM desc ");
            //第三条语句
            strSql.Append($"select datename(year,IDTM) as IDTM,MAXS,MINS from {File_Schema}ST_SSTAT_R ");
            strSql.Append(" where STCD = @stcd");
            strSql.Append(" order by IDTM desc ");
            //第四条语句
            strSql.Append($"select datename(year,IDTM) as IDTM,ACCJL from {File_Schema}ST_JLSTAT_R ");
            strSql.Append(" where STCD = @stcd");
            strSql.Append(" order by IDTM desc ");
            //第五条语句
            strSql.Append($"select datename(year,IDTM) as IDTM,STW from {File_Schema}ST_SEDRF_R ");
            strSql.Append(" where STCD = @stcd");
            strSql.Append(" order by IDTM desc ");
            //第六条语句
            strSql.Append($"select datename(year,IDTM) as IDTM,ACCP from {File_Schema}ST_PPSTAT_R ");
            strSql.Append(" where STCD = @stcd");
            strSql.Append(" order by IDTM desc ");

            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.Table = multi.Read<dynamic>().AsList();
                data.Table1 = multi.Read<dynamic>().AsList();
                data.Table2 = multi.Read<dynamic>().AsList();
                data.Table3 = multi.Read<dynamic>().AsList();
                data.Table4 = multi.Read<dynamic>().AsList();
                data.Table5 = multi.Read<dynamic>().AsList();
            }
            return data;
        }
        #endregion
		
        #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetZQRLData(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            string sqlInnerText = $"select a.STNM,b.BGTM,b.LNNM,b.PTNO,b.Z,b.Q,b.ZMEA,b.QMEA from {RTDB_Schema}ST_STBPRP_B a, {File_Schema}TBL_ZQRL b where"
                + " b.STCD = @stcd and a.STCD = b.STCD ";
            sqlInnerText += " order by b.BGTM,b.PTNO"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// 获取测站有数据的年份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStationZQRLYear(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            string sqlInnerText = $"select distinct rtrim(LNNM) as LNNM,rtrim(LNNM)+'年' as LNNMTEXT  from {File_Schema}tbl_zqrl where stcd=@stcd ";
            sqlInnerText += " order by LNNM asc"; ;
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        ///获取测站选择年份的数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetZQRLYearsData(string stcd, string years)
        {
            if (!string.IsNullOrWhiteSpace(years))
            {
                var sqlParams = new DynamicParameters();
                sqlParams.Add("stcd", stcd);
                sqlParams.Add("year", years.Split(','));
                string sqlInnerText = $"select a.STNM,b.BGTM,b.LNNM,b.PTNO,b.Z,b.Q,b.ZMEA,b.QMEA from  {RTDB_Schema}ST_STBPRP_B a, {File_Schema}TBL_ZQRL b "
                    + " where b.STCD = @stcd and b.LNNM in @year and a.STCD = b.STCD ";
                sqlInnerText += " order by b.BGTM,b.PTNO"; ;
                using (var db = database.Connection)
                {
                    return db.Query<dynamic>(sqlInnerText, sqlParams);
                }
            }
            else
            {
                var sqlParams = new DynamicParameters();
                sqlParams.Add("stcd", stcd);
                string sqlInnerText = $"select a.STNM,'' as BGTM,'' as LNNM,'' as PTNO,'' as Z, '' as Q,'' as ZMEA,'' as QMEA from   {RTDB_Schema}ST_STBPRP_B a where STCD=@stcd";
                using (var db = database.Connection)
                {
                    return db.Query<dynamic>(sqlInnerText, sqlParams);
                }
            }
           
        }
        #endregion
       
		#region 设施设备
        /// <summary>
        /// 获取设施设备分类的信息树数据
        /// </summary>
        /// <returns></returns>
        public dynamic GetFacEqTreeData()
        {
            //第一条语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select * from {File_Schema}TO_FACEQ_CLASS where substring(FIRCLASS,3,6) = '000000'");

            //第二条语句
            strSql.Append($"select * from {File_Schema}TO_FACEQ_CLASS where substring(FIRCLASS,3,2) <> '00' and substring(FIRCLASS,5,4) = '0000'");
            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString()))
            {
                data.Table = multi.Read<dynamic>().AsList();
                data.Table1 = multi.Read<dynamic>().AsList();
            }

            return data;
        }
        /// <summary>
        /// 根据站码和表名获取相关的表信息和测站的相关设施设备信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public dynamic GetFacEqFieldsAndData(string stcd, string tableName)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("tableName", tableName);
            sqlParams.Add("stcd", stcd);

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select uc.COLUMN_NAME,uc.comments,ut.DATA_TYPE,ut.DATA_LENGTH,ut.NULLABLE from user_tab_columns  ut inner JOIN user_col_comments uc on ut.TABLE_NAME  = uc.table_name and ut.COLUMN_NAME = uc.column_name where ut.Table_Name=@tableName ");
            strSql.Append("SELECT B.name AS COLUMN_NAME,C.value AS COMMENTS,D.type as DATA_TYPE, D.length as DATA_LENGTH, D.NULLABLE as NULLABLE ");
            strSql.Append($" FROM {File_DBName}sys.tables A");
            strSql.Append($" left join  {File_DBName}sys.columns B ON B.object_id = A.object_id");
            strSql.Append($" LEFT JOIN {File_DBName}sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id"); 
            strSql.Append(" INNER JOIN"); 
            strSql.Append($" (select a.id, a.colid, a.length, a.name, b.name as type, (case a.isnullable  when 1 then 'Y' else 'N' end) as NULLABLE from {File_Schema}syscolumns a, {File_Schema}sysTypes b where a.xtype = b.xtype) D"); 
            strSql.Append(" on D.id = B.object_id and d.colid = B.column_id");
            strSql.Append(" WHERE A.name =@tableName order by  B.column_id  asc ");
            //第二条语句
            //strSql.Append(" select * from @tableName  where STCD = @stcd");
            strSql.Append(" select * from " + File_Schema + tableName + " where STCD =@stcd");
            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.Table = multi.Read<dynamic>().AsList();
                data.Table1 = multi.Read<dynamic>().AsList();
            }

            return data;
        }
        #endregion
        #endregion
    }
}
