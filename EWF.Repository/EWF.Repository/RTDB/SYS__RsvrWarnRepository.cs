using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository.SysManage;
using EWF.Util.Options;
using EWF.Util;
using EWF.Entity;
using EWF.Util.Page;

namespace EWF.Repository
{
    public class SYS__RsvrWarnRepository: DefaultRepository, ISYS__RsvrWarnRepository
    {
        public SYS__RsvrWarnRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
        }

        public Page<dynamic> GetRsvrWarnData(int pageIndex, int pageSize, string stnm, int type, string addvcd)
        {
            //string sqlInnerText = "select A.RVNM, A.STCD,A.STNM AS STNM from EW_NPW.DBO.ST_STBPRP_V A where sttp='RR' and type=" + type + " and addvcd='" + addvcd + "'";
            // //参数列表
            //var sqlParams = new DynamicParameters();

            //if (!string.IsNullOrEmpty(stnm))
            //{                
            //    sqlInnerText += string.Format(" AND STNM like '%{0}%'", stnm);
            //    //sqlParams.Add("STNM", stnm);
            //}


            //var tableName = "(" + sqlInnerText + ") aa left join ST_RSVRFSR_B B on aa.stcd=b.stcd ";
            //var flied = "AA.RVNM,AA.STCD,AA.STNM,ACTYR,BGMD,EDMD,FSLTDZ,(CASE WHEN FSTP = '1' THEN '主汛期' WHEN FSTP = '2' THEN '后汛期' WHEN FSTP = '3' THEN '过渡期' WHEN FSTP = '4' THEN '其他'    ELSE '' END) AS FSTP";
            //var where = "1=1";
            //var orderby = "ACTYR DESC";

            //只显示今年的数据
            //主汛期
            string sql1 = "SELECT AA.RVNM,AA.STCD,AA.STNM,ACTYR,BGMD,EDMD,FSLTDZ,'1' as FSTP,'主汛期' as FSTPNAME FROM (select A.RVNM, A.STCD,A.STNM AS STNM from "+Default_Schema  +"ST_STBPRP_V A where sttp='RR' and type=" + type + " and addvcd='" + addvcd + "') aa left join "+RTDB_Schema+"ST_RSVRFSR_B B on aa.stcd=b.stcd and fstp='1' and actyr=" + DateTime.Now.Year;
            //后汛期
            string sql2 = "SELECT AA.RVNM,AA.STCD,AA.STNM,ACTYR,BGMD,EDMD,FSLTDZ,'2' as FSTP,'后汛期' as FSTPNAME FROM (select A.RVNM, A.STCD,A.STNM AS STNM from " + Default_Schema + "ST_STBPRP_v A where sttp='RR' and type=" + type + " and addvcd='" + addvcd + "') aa left join " + RTDB_Schema + "ST_RSVRFSR_B B on aa.stcd=b.stcd and fstp='2' and actyr=" + DateTime.Now.Year;
            //过渡期
            string sql3 = "SELECT AA.RVNM,AA.STCD,AA.STNM,ACTYR,BGMD,EDMD,FSLTDZ,'3' as FSTP,'过渡期' as FSTPNAME FROM (select A.RVNM, A.STCD,A.STNM AS STNM from " + Default_Schema + "ST_STBPRP_v A where sttp='RR' and type=" + type + " and addvcd='" + addvcd + "') aa left join " + RTDB_Schema + "ST_RSVRFSR_B B on aa.stcd=b.stcd and fstp='3' and actyr=" + DateTime.Now.Year;
            //其他
            string sql4 = "SELECT AA.RVNM,AA.STCD,AA.STNM,ACTYR,BGMD,EDMD,FSLTDZ,'4' as FSTP,'其他' as FSTPNAME FROM (select A.RVNM, A.STCD,A.STNM AS STNM from " + Default_Schema + "ST_STBPRP_v A where sttp='RR' and type=" + type + " and addvcd='" + addvcd + "') aa left join " + RTDB_Schema + "ST_RSVRFSR_B B on aa.stcd=b.stcd and fstp='4' and actyr=" + DateTime.Now.Year;

            string sql = sql1 + " union " + sql2 + " union " + sql3 + " union " + sql4;
            var tableName = "(" + sql + ")a";
            var flied = "RVNM,STCD,STNM,isnull(ACTYR,year(getdate())) as ACTYR,BGMD,EDMD,FSLTDZ,FSTP,FSTPNAME";
            var where = "where 1=1";
            if (!stnm.IsEmpty())
                where += " and stnm like '%" + stnm + "%'";
            var orderby = "stcd,fstp";

            var page = database.GetListPaged<dynamic>(pageIndex, pageSize, tableName, flied, where, orderby, null);
            return page;
        }

        public string UpdateData(SYS_ST_RSVRFSR_B model)
        {
            //先判断存在不存在，存在更新，不存在插入
            var sql = "";
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", model.STCD);
            sqlParams.Add("actyr", model.ACTYR);
            sqlParams.Add("fstp", model.FSTP);
            //sqlParams.Add("bgmd", model.BGMD);
            var condition = " where STCD=@stcd and ACTYR=@actyr and FSTP=@fstp";// and BGMD=@bgmd";
            int count = database.Count<SYS_ST_RSVRFSR_B>(condition, sqlParams);
            if (count > 0)
            {
                sql = $"update {RTDB_Schema}ST_RSVRFSR_B set BGMD='" + model.BGMD + "',EDMD='" + model.EDMD + "',FSLTDZ=" + model.FSLTDZ + " where stcd='" + model.STCD + "' and ACTYR=" + model.ACTYR + " and FSTP='" + model.FSTP + "'";
            }
            else
            {
                sql = $"INSERT INTO {RTDB_Schema}ST_RSVRFSR_B(STCD,ACTYR,BGMD,EDMD,FSLTDZ,FSTP)VALUES('" + model.STCD + "'," + model.ACTYR + ",'" + model.BGMD + "','" + model.EDMD + "'," + model.FSLTDZ + ",'" + model.FSTP + "')";
            }
            int result = database.ExecuteBySql(sql);
            if (result > 0)
                return "修改成功";
            else
                return "修改失败";

        }
    }
}
