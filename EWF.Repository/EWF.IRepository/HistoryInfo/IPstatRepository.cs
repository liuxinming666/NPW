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

namespace EWF.IRepository
{
    public interface IPstatRepository : IDependency
    {
        #region 降水量累计-统计
        /// <summary>
        /// 降水量累计-统计-多日-多日均值表
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日累计</returns>
        IEnumerable<ST_PSTATEntity> GetDayData(string STCD, string sdate, string edate, int type, string addvcd);
        /// <summary>
        /// 降水量累计-统计-多日-实时表
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日累计</returns>
        IEnumerable<ST_PSTATEntity> GetDayData_ByPPTN(string STCD, string sdate, string edate, int type, string addvcd);
        /// <summary>
        /// 降水量累计-统计-旬月
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内旬月累计</returns>
        IEnumerable<ST_PSTATEntity> GetMonthData(string STCD, string sdate, string edate, int type, string addvcd);
        /// <summary>
        /// 降水量累计-统计-年
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>指定年份12个月的月累计</returns>
        IEnumerable<ST_PSTATEntity> GeYearData(string STCD, string sdate, string edate, int type, string addvcd);

        #endregion

        #region 降水量累计-对比分析
        /// <summary>历史同期降水量对比分析-旬月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="STTDRCD">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <param name="type">对比类型 1：旬，2：月</param>
        /// <returns>时段内旬月均值</returns>
        dynamic GetMonthComparativeData(string STCD, string STTDRCD, string sdate, string edate, string sdate_history, string edate_history);

        #endregion
    }
}