/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：lw
 * 日  期：2019/5/24 16:48:11
 * 描  述：IRsvrService
 * *********************************************/
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    public interface IRsvrService : IDependency
    {
        Page<dynamic> GetReadData(int pageIndex, int pageSize, string STNM);

        /// <summary>
        /// 返回最新水库水情数据
        /// add by lw
        /// Date:2019-05-23 13:00
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetLatestRsvrData(string startDate,string endDate,string addvcd, string type);

        /// <summary>
        /// 返回水库水情信息
        /// add by lw
        /// Date:2019-05-23 14:00
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Page<dynamic> GetRsvrData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type);

        /// <summary>
        /// 获取水库水情信息
        /// </summary>
        /// add by lw
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<dynamic> GetRsvrav_avg(int sttdrcd, string stcd, string startDate, string endDate);

        /// <summary>
        /// 获取水库水位过程线
        /// </summary>
        /// add by lw
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<dynamic> GetRsvr_Line(string stcd, string startDate, string endDate);
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
