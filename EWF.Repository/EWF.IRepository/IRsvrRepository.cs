using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IRepository
{
    public interface IRsvrRepository: IDependency
    {
        Page<dynamic> GetReadData(int pageIndex, int pageSize, string STNM);

        /// <summary>
        /// 获取指定时间段内最新水库水情
        /// add by lw
        /// Date:2019-05-24 23:00
        /// </summary>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区编码</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetLatestRsvr(string startDate, string endDate, string addvcd, string type);

        /// <summary>
        /// 查询水库水情信息
        /// add by lw
        /// Date:2019-05-24 10:00
        /// </summary>
        /// <param name="stcds">站码列表,当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区编码</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        Page<dynamic> GetRsvrData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type);

        /// <summary>
        /// 查询水库水情信息-未分页
        /// add by lw
        /// Date:2019-05-24 17:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<dynamic> GetRsvrData(string stcd, string startDate, string endDate);

        /// <summary>
        /// 查询水库水情均值
        /// add by lw
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRsvrav_avg(int sttdrcd, string stcds, string startDate, string endDate);

        /// <summary>
        /// 查询水库水情过程线
        /// add by lw
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRsvr_Line(string stcds, string startDate, string endDate);
        /// <summary>
        /// 首页查询8点水库水情过程线信息
        /// add by qlj
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRsvrLineEight(string stcd, string startDate, string endDate);
    }
}
