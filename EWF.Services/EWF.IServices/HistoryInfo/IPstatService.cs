/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/22 17:24:18
 * 描  述：IPstatService
 * *********************************************/
using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface IPstatService : IDependency
    {
        #region 降水量统计
        /// <summary>
        /// 水情均值-统计-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日均值</returns>
        IEnumerable<dynamic> GetDayData(string STCD, string sdate, string edate, int type, string addvcd, ref string datasrc);
        /// <summary>
        /// 水情均值-统计-旬月
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="smonth">开始月份</param>
        /// <param name="emonth">结束月份</param>
        /// <returns>时段内旬月均值</returns>
        IEnumerable<dynamic> GetMonthData(string STCD, string smonth, string emonth, int type, string addvcd, ref string datasrc);
        /// <summary>
        /// 水情均值-统计-年
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="year">年份</param>
        /// <returns>指定年份12个月的月均值</returns>
        DataTable GeYearData(string STCD, string year, int type, string addvcd, ref string datasrc);

        #endregion

        #region 降水量对比分析
        /// <summary>历史同期降水量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内月均值</returns>
        IEnumerable<dynamic> GetMonthComparativeData(string STCD, string type, string sdate, string edate, string year, ref string datasrc);
        /// <summary>历史同期降水量对比分析-旬</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内旬均值</returns>
        IEnumerable<dynamic> GetTenComparativeData(string STCD, string type, string sdate, string edate, string sten, string eten, string year, ref string datasrc);
        #endregion


    }
}
