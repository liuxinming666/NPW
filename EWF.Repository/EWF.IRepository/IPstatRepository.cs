/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：RiverRepository
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;

namespace EWF.EWF.IRepository
{
    public interface IPstatRepository : IDependency
    {
        // 降水量统计
        IEnumerable<ST_PSTATEntity> GetDayData(string STCD, string sdate, string edate);
        IEnumerable<ST_PSTATEntity> GetDayData_ByPPTN(string STCD, string sdate, string edate);
        IEnumerable<ST_PSTATEntity> GetMonthData(string STCD, string sdate, string edate);
        IEnumerable<ST_PSTATEntity> GeYearData(string STCD, string sdate, string edate);

        // 降水量对比分析
        /// <summary>历史同期降水量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="end_history">对比结束日期</param>
        /// <returns>时段内月均值</returns>
        IEnumerable<ST_PSTATEntity> GetMonthComparativeData(string STCD, string sdate, string edate, string sdate_history, string edate_history);
    }
}