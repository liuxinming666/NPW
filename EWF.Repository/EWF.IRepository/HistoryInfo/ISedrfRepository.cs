/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：ISedrfRepository
 * *********************************************/
using EWF.Entity;
using EWF.Util;
using System.Collections.Generic;
using System.Data;

namespace EWF.IRepository
{
    public interface ISedrfRepository : IDependency
    {
        #region 径流量输沙量-统计
        /// <summary>
        /// 径流量输沙量-统计-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日均值</returns>
        IEnumerable<ST_SEDRFEntity> GetDayData(string STCD, string addvcd, string type, string sdate, string edate);
        /// <summary>
        /// 径流量输沙量-统计-旬月
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内旬月均值</returns>
        IEnumerable<ST_SEDRFEntity> GetMonthData(string STCD, string addvcd, string type, string sdate, string edate);
        /// <summary>
        /// 径流量输沙量-统计-年
        /// </summary>
        /// <param name="STCD"></param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>指定年份12个月的月累计</returns>
        IEnumerable<ST_SEDRFEntity> GeYearData(string STCD, string addvcd, string type, string sdate, string edate);
        #endregion

        #region 径流量输沙量-对比分析
        /// <summary>
        /// 径流量-输沙量多日对比分析
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="edate_history">对比结束日期</param>
        /// <returns>{real：分析数据，history：对比数据}</returns>
        dynamic GetDayData_Comparative(string STCD, string addvcd, string type, string sdate, string edate, string sdate_history,string edate_history);
      
        #endregion

    }
}
