using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface  IIcejamService : IDependency
    {
        /// <summary>
        /// 查询多站水温数据-未分页
        /// add by Zhao
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetSingleTempData(string stcd, string startDate, string endDate);

        /// <summary>
        /// 查询多站水文数据-未分页
        /// add by SUN
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        string GetTempDataByMultiStcds(string stcds,string sqnm, string startDate, string endDate);
        List<dynamic> GetSearchKeywords(string keyword);

        /// <summary>
        /// 获取凌情信息
        /// </summary>
        /// add by lw
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        string GetIceDate(string stcd, string startDate, string endDate,string  addvcd,string type);

        /// <summary>
        /// 获取凌情动态
        /// </summary>
        /// add by lw
        /// <param name="sDate"></param>
        /// <returns></returns>
        DataTable GetIceJamDynamic(string sDate);

        /// <summary>
        /// 获取凌情动态关注的水情站点信息
        /// </summary>
        /// add by lw
        /// <param name="Sdate"></param>
        /// <returns></returns>
        DataTable GetIceJam_River(string Sdate);

        DataTable GetIceJam_LQDT(string Sdate);
    }
}
