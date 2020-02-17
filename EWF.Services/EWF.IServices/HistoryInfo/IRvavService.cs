/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/28 15:56:58
 * 描  述：IRvavService
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.IServices
{
    public interface IRvavService:IDependency
    {
        Page<ST_RVAVEntity> GetPageData(int pageIndex, int pageSize, string STCD, string STTDRCD, string addvcd, string type);
        int Count(string STCD);
        ST_RVAVEntity Get(Int32 ID);
        bool Insert(ST_RVAVEntity entity);
        bool Update(ST_RVAVEntity entity);
        bool Delete(ST_RVAVEntity entity);
        bool DeleteList(object whereConditions);



        /// <summary>
        /// 水情均值-统计-多日
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>时段内单日均值</returns>
        IEnumerable<ST_RVAVEntity> GetDayData(string STCD, string Addvcd, string type, string sdate, string edate, ref string datasrc);
        /// <summary>
        /// 水情均值-统计-旬月
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="smonth">开始月份</param>
        /// <param name="emonth">结束月份</param>
        /// <returns>时段内旬月均值</returns>
        IEnumerable<dynamic> GetMonthData(string STCD, string Addvcd, string type, string smonth, string emonth, ref string datasrc);

        /// <summary>
        /// 水情均值-统计-年
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="year">年份</param>
        /// <returns>指定年份12个月的月均值</returns>
        DataTable GeYearData(string STCD, string Addvcd, string type, string year, ref string datasrc);

        /// <summary>
        /// 水情历史同期多日均值对比分析
        /// </summary>
        /// <param name="STCD">站名</param>
        /// <param name="avgType">均值类型</param>
        /// <param name="sdate">分析开始日期</param>
        /// <param name="edate">分析结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>对比数据表</returns>
        IEnumerable<dynamic> GetData_Comparative(string STCD, string Addvcd, string type, string avgType, string sdate, string edate, string year);
    }
}
