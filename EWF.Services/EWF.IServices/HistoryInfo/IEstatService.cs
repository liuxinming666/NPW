/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/29 18:02:30
 * 描  述：IEstatService
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Entity;
using EWF.Util;

namespace EWF.IServices
{
    public interface IEstatService : IDependency
    {
        IEnumerable<ST_ESTATEntity> GetDayData(string sTCD, string sdate, string edate, int type, string addvcd, ref string datasrc);
        IEnumerable<dynamic> GetMonthData(string sTCD, string smonth, string emonth, int type, string addvcd, ref string datasrc);
        DataTable GeYearData(string sTCD, string year, int type, string addvcd, ref string datasrc);
        #region 蒸发量对比分析
        /// <summary>历史同期蒸发量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内月均值</returns>
        IEnumerable<dynamic> GetData_MMonthEV(string STCD, string type, string sdate, string edate, string year, ref string datasrc);
        /// <summary>历史同期蒸发量对比分析-旬</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内旬均值</returns>
        IEnumerable<dynamic> GetData_MXunEV(string STCD, string type, string sdate, string edate, string sten, string eten, string year, ref string datasrc);
        #endregion
    }
}
