/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijuan
 * 日  期：2019/5/30 16:36:04
 * 描  述：WaterFloodRepository
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
    public class WaterFloodRepository: DefaultRepository, IWaterFloodRepository
    {
		private readonly string ST_STBPRP_V;
		private readonly string PrimaryTableName;
		public WaterFloodRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			PrimaryTableName = $"{File_Schema}ST_FSPS_R";
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
		}

		public IEnumerable<ST_FSPS_REntity> GetWaterFloodData(string STCD, string sdate, string edate)
        {
            #region 参数校验
            if(STCD.IsEmpty())
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
            #endregion

            var sqlParams = new DynamicParameters();
			sqlParams.Add(nameof(STCD), STCD);
			sqlParams.Add(nameof(sdate), sdate);
            sqlParams.Add(nameof(edate), edate);

            var strSql = new StringBuilder();
			strSql.Append("SELECT tbb.STNM,tba.STCD,TM,Z,Q,S ");
            strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd ");
            strSql.Append("WHERE (1=1) ");
			strSql.Append("AND (tba.STCD=@STCD) ");
			strSql.Append("AND (tm >@sdate) AND (tm<@edate) ");
            strSql.Append("ORDER BY tba.STCD ASC,TM ASC ");

            return database.Connection.Query<ST_FSPS_REntity>(strSql.ToString(), sqlParams);
        }

       
        public dynamic GetWaterFloodMutiData(string STCD, string sdate, string edate)
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
            #endregion

            var sqlParams = new DynamicParameters();
            sqlParams.Add(nameof(STCD), STCD.Split(','));
            sqlParams.Add("sdate", sdate);
            sqlParams.Add("edate", edate);

            var strSql = new StringBuilder();
			//第一条语句
			strSql.Append("SELECT tbb.STNM,tba.STCD,TM,Z,Q ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd ");
			strSql.Append("WHERE tba.STCD in @STCD AND tm >@sdate AND tm<@edate ");
            strSql.Append("ORDER BY TM ASC,tba.STCD ASC ");
            //第二条语句
            strSql.Append("SELECT tbb.STNM,tba.STCD,TM,S ");
			strSql.Append($"FROM {PrimaryTableName} tba JOIN {ST_STBPRP_V} tbb ON tba.stcd = tbb.stcd ");
			strSql.Append("WHERE tba.STCD in @STCD AND tm >@sdate AND tm<@edate and S is not null ");
            strSql.Append("ORDER BY TM ASC,tba.STCD ASC ");
            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.z = multi.Read<dynamic>();
                data.s = multi.Read<dynamic>();
            }

            return data;// database.Connection.Query<dynamic>(strSql.ToString(), sqlParams);
        }
    }
}
