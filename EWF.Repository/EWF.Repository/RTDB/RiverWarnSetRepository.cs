using EWF.Data.Repository;
using EWF.Entity;
using EWF.IRepository;
using EWF.Util;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Repository
{
    public class RiverWarnSetRepository : DefaultRepository, IRiverWarnSetRepository
    {
		private readonly string ST_STBPRP_V;
		private readonly string PrimaryTableName;
		public RiverWarnSetRepository(IOptionsSnapshot<DbOption> options):base(options)
        {
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			PrimaryTableName = $"{RTDB_Schema}ST_RVFCCH_B";
		}
        public Page<dynamic> GetRiverWarnData(int pageIndex, int pageSize, string stnm, int type, string addvcd)
        {
            //主汛期
            string sql = $"SELECT A.STCD,A.STNM,A.STTP,B.WRZ,B.WRQ,B.GRZ,B.GRQ from {ST_STBPRP_V} A LEFT JOIN {PrimaryTableName} B ON A.STCD=B.STCD where (sttp='ZQ' OR STTP='ZZ') and type=" + type + " and addvcd='" + addvcd + "'";
            var tableName = "(" + sql + ")a";
            var flied = "STCD,STNM,WRZ,WRQ,GRZ,GRQ";
            var where = "1=1";
            if (!stnm.IsEmpty())
                where += " and stnm like '%" + stnm + "%'";
            var orderby = "stcd";
            var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby, null);
            return page;
        }
        public string UpdateData(ST_RVFCCH_B model)
        {
            //先判断存在不存在，存在更新，不存在插入
            var sql = "";
            var sqlParams = new Dapper.DynamicParameters();
            sqlParams.Add("stcd", model.STCD);
            var condition = " where STCD=@stcd";
            int count = database.Count<ST_RVFCCH_B>(condition, sqlParams);
            if (count > 0)
            {
                sql = $"update {PrimaryTableName} set WRZ=" + model.WRZ + ",WRQ=" + model.WRQ + ",GRZ=" + model.GRZ + ",GRQ=" + model.GRQ + " where stcd='" + model.STCD + "'";
            }
            else
            {
                sql = $"INSERT INTO {PrimaryTableName} (STCD,WRZ,WRQ,GRZ,GRQ)VALUES('" + model.STCD + "'," + model.WRZ + "," + model.WRQ + "," + model.GRZ + "," + model.GRQ + ")";
            }
            int result = database.ExecuteBySql(sql);
            if (result > 0)
                return "修改成功";
            else
                return "修改失败";

        }

    }
}
