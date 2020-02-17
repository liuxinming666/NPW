/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijua
 * 日  期：2019/5/27 12:36:04
 * 描  述：WatchWarnRepository
 * *********************************************/
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Repository
{
    public class WatchWarnRepository:DefaultRepository, IWatchWarnRepository
    {
		public WatchWarnRepository(IOptionsSnapshot<DbOption> options) : base(options) { }
        /// <summary>
        /// 获取预警信息数据
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">类型：1行政区划2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        public DataTable GetWarnWarnData(string startDate, string endDate, int type, string addvcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("@type", type);
            sqlParams.Add("@addvcd", addvcd);
            //先执行存储过程，生成预警数据
            using (var db = database.Connection)
            {
                db.Execute("[dbo].[PR_WARN_LIST]", sqlParams, null, null, CommandType.StoredProcedure); 
            }
            string strSql = "select a.stcd,ltrim(rtrim(a.stnm)) as stnm,RIGHT(CONVERT(VARCHAR(16),tm,120),11) as tm,drp,sumname,thname,sjdj,a.sttp,qjfz,z,q,a.lggd,a.lttd,legend,sjjj,wptn from tbl_event_list a,st_stbprp_v b where a.stcd=b.stcd and b.type=" + type + " and b.addvcd='" + addvcd + "' and jzsj>'" + startDate + "' and jzsj<='" + endDate + "' order by sjdj,sttp,duration,sumname,thname,stcd,tm desc ";
            DataTable dtWatchWarn = database.FindTable(strSql);
            return dtWatchWarn;
        }
        /// <summary>
        /// 获取详细的预警信息数据
        /// </summary>
        /// <param name="sname">预警类型名称</param>
        /// <param name="tname">具体预警类别</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="sttp">测站类型</param>
        /// <param name="qjfz"></param>
        /// <returns></returns>
        /// <returns></returns>
        public DataTable GetWarnWarnDetailData(string sname, string tname, string startDate, string endDate,string sttp, string qjfz)
        {
            string sql = "";
            if (sttp == "ZZ" || sttp == "ZQ")
            {
                sql = "select a.stcd,a.stnm,a.sttp,a.lggd,a.lttd,legend,sjjj,qjfz,drp,z,q,wptn,RIGHT(CONVERT(VARCHAR(16),tm,120),11) as tm from tbl_event_list a where sumname='" + sname + "' and thname='" + tname + "' and jzsj>'" + startDate + "' and jzsj<='" + endDate + "' order by stcd,tm desc";
            }
            else if (sttp == "PP")
            {
                sql = "select a.stcd,a.stnm,a.sttp,a.lggd,a.lttd,legend,sjjj,qjfz,wptn as wth,RIGHT(CONVERT(VARCHAR(16),a.tm,120),11) as tm "
                       + "from tbl_event_list a,("
                       + "select a1.stcd,max(a1.tm) as tm,a1.drp,a1.sumname,a1.thname from tbl_event_list a1,"
                       + "(select stcd,max(drp) as drp,sumname,thname from tbl_event_list "
                       + "where sumname='" + sname + "' and thname='" + tname + "' and "
                       + "jzsj>'" + startDate + "' and jzsj<='" + endDate + "' "
                       + "group by stcd,sumname,thname) a2 "
                       + "where a1.stcd=a2.stcd  and a1.sumname=a2.sumname and a1.thname=a2.thname "//and a1.drp = a2.drp"
                       + "group by a1.stcd,a1.drp,a1.sumname,a1.thname"
                       + ") b "
                       + "where a.stcd=b.stcd and a.tm=b.tm and a.sumname=b.sumname and a.thname=b.thname";// and a.drp=b.drp ";
            }
            else
            {
                sql = "select stcd,stnm,sttp,lggd,lttd,legend,sjjj,qjfz,z as rz,q as w,wptn as rwptn,RIGHT(CONVERT(VARCHAR(16),tm,120),11) as tm from tbl_event_list a where sumname='" + sname + "' and thname='" + tname + "' and jzsj>'" + startDate + "' and jzsj<='" + endDate + "' order by stcd,tm desc";
            }
            DataTable dt = database.FindTable(sql);
            return dt;
        }

    }
    
}
