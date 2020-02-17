using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Entity;
using EWF.Util;

namespace EWF.IServices
{
    public interface ITmpavService : IDependency
    {
        /// <summary>
        /// 查询水温气温统计分析数据-未分页
        /// add by Zhao
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetTempComparativeData(string stcd, string startDate, string endDate);
        #region 水温气温对比分析
        /// <summary>历史同期蒸发量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内月均值</returns>
        IEnumerable<dynamic> GetData_MMonthEV(string STCD, string type, string sdate, string edate, string year, ref string datasrc);
        #endregion
    }
}
