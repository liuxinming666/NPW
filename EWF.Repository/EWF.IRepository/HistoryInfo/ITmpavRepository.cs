using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;

namespace EWF.IRepository
{
    public interface ITmpavRepository : IDependency
    {
        /// <summary>
        /// 查询单站水温信息-未分页
        /// add by Zhao
        /// Date:2019-05-23 17:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetTempComparativeData(string stcd, string startDate, string endDate);
        #region 历史同期水温气温-对比分析
        /// <summary>历史同期蒸发量对比分析-旬月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="STTDRCD">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <param name="type">对比类型 1：旬，2：月</param>
        /// <returns>时段内旬月均值</returns>
        dynamic GetData_MMonthEV(string STCD, string STTDRCD, string sdate, string edate, string sdate_history, string edate_history);
        #endregion
    }

}
