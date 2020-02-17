using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.IRepository
{
    public interface IIcejamRepository : IDependency
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
        IEnumerable<dynamic> GetSingleTempData(string stcd, string startDate, string endDate);

        /// <summary>
        /// 查询多站水温信息-未分页
        /// add by Zhao
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetTempDataByMultiStcds(string stcds, string startDate, string endDate);
        IEnumerable<dynamic> GetSearchKeywords(string keyword);

        /// <summary>
        /// 查询实时凌情信息
        /// add by lw
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="addvcd">行政区编码</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        DataTable GetIceData(string stcds, string startDate, string endDate, string addvcd, string type);

        /// <summary>
        /// 获取凌情动态 oracle
        /// </summary>
        /// add by lw
        /// <param name="sDate"></param>
        /// <returns></returns>
        DataTable GetIceJamDynamic(string sDate);

        /// <summary>
        /// 获取凌情动态 sql版本
        /// </summary>
        /// add by lw
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable GetIceJamDynamic_sql(string startDate, string endDate);

        /// <summary>
        /// 获取凌情动态中关注的水情信息
        /// </summary>
        /// add by lw
        /// <param name="Sdate"></param>
        /// <returns></returns>
        DataTable GetIceJam_River(string Sdate);

        DataTable GetIceJam_LQDT(string Sdate);
    }
}
