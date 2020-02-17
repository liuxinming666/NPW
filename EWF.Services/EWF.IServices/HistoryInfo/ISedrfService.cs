/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/25 17:24:18
 * 描  述：ISedrfService
 * *********************************************/
using EWF.Util;
using EWF.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ISedrfService : IDependency
    {
        #region 径流量输沙量- 统计
        /// <summary>
        /// 径流量输沙量-统计-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日累计值</returns>
        IEnumerable<ST_SEDRFDayViewModel> GetDayData(string STCD, string Addvcd, string type, string sdate, string edate, ref string datasrc);
        /// <summary>
        /// 径流量输沙量-统计-旬月
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="smonth">开始月份</param>
        /// <param name="emonth">结束月份</param>
        /// <returns>时段内旬月累计值</returns>
        IEnumerable<dynamic> GetMonthData(string STCD, string Addvcd, string type, string smonth, string emonth, ref string datasrc);
        /// <summary>
        /// 径流量输沙量-统计-年
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="year">年份</param>
        /// <returns>指定年份12个月的月累计值</returns>
        DataTable GeYearData(string STCD, string Addvcd, string type, string year, ref string datasrc);
        #endregion

        #region 径流量输沙量 对比分析
        /// <summary>
        /// 水情均值多日对比分析-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>对比数据表</returns>
        IEnumerable<dynamic> GetDayData_Comparative(string STCD, string Addvcd, string type, string sdate, string edate,string year, ref string datasrc);
        #endregion
    }
}
