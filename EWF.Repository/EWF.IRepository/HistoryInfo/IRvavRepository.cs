/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：IRvavRepository
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.IRepository
{
    public interface IRvavRepository : IRepositoryBase<ST_RVAVEntity>,IDependency
    {
        #region 水情均值-管理维护
        Page<ST_RVAVEntity> GetPageData(int pageIndex, int pageSize, string STCD, string STTDRCD, string addvcd, string type);
        #endregion

        #region 水情均值-统计
        /// <summary>
        /// 水情均值-统计-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日均值</returns>
        IEnumerable<ST_RVAVEntity> GetDayData(string STCD, string Addvcd, string Type, string sdate, string edate);
        /// <summary>
        /// 水情均值-统计-多日-实时表
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日均值</returns>
        IEnumerable<ST_RIVEREntity> GetDayData_River(string STCD, string Addvcd, string Type, string sdate, string edate);
        /// <summary>
        /// 水情均值-统计-旬月
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内旬月均值</returns>
        IEnumerable<ST_RVAVEntity> GetMonthData(string STCD, string Addvcd, string Type, string sdate, string edate);
        /// <summary>
        /// 水情均值-统计-年
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>指定年份12个月的月均值</returns>
        IEnumerable<ST_RVAVEntity> GeYearData(string STCD,string Addvcd,string Type, string sdate, string edate);

        #endregion

        #region 水情均值-对比分析 
        /// <summary>
        /// 水情多日均值对比分析-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="avgType">均值类型</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="edate_history">对比结束日期</param>
        /// <returns>{real：分析数据，history：对比数据}</returns>
        dynamic GetData_Comparative(string STCD, string addvcd, string type, string avgType, string sdate, string edate, string sdate_history, string edate_history);

        /// <summary>
        /// 水情均值多日对比分析-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="sdate_history">对比开始日期</param>
        /// <param name="edate_history">对比结束日期</param>
        /// <returns>{real：分析数据，history：对比数据}</returns>
        dynamic GetDayData_Comparative(string STCD, string addvcd, string type, string sdate, string edate, string sdate_history, string edate_history);
        #endregion
    }
}
